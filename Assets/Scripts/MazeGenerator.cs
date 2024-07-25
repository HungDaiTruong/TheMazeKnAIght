using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject startPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab; // Prefab of the player character
    public GameObject coinPrefab; // Prefab of the coin
    public CameraController cameraController; // Reference to the current camera script
    private GameObject currentPlayer; // Reference to the current player instance
    private int[,] maze;
    private System.Random rand = new System.Random();
    private Vector2Int startCell;
    private Vector2Int goalCell;
    private int currentLevel = 1;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
        PlaceStartAndGoal();
        PlaceCoins();
        SpawnPlayer();
    }

    public void GenerateMaze()
    {
        maze = new int[width, height];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int currentCell = new Vector2Int(0, 0);
        maze[currentCell.x, currentCell.y] = 1;
        stack.Push(currentCell);

        while (stack.Count > 0)
        {
            currentCell = stack.Pop();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count > 0)
            {
                stack.Push(currentCell);

                Vector2Int chosenNeighbor = neighbors[rand.Next(neighbors.Count)];
                Vector2Int between = (currentCell + chosenNeighbor) / 2;
                maze[chosenNeighbor.x, chosenNeighbor.y] = 1;
                maze[between.x, between.y] = 1;
                stack.Push(chosenNeighbor);
            }
        }

        // Set start and goal positions
        startCell = new Vector2Int(0, 0);
        goalCell = FindGoalCell();
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (cell.x > 1 && maze[cell.x - 2, cell.y] == 0) neighbors.Add(new Vector2Int(cell.x - 2, cell.y));
        if (cell.x < width - 2 && maze[cell.x + 2, cell.y] == 0) neighbors.Add(new Vector2Int(cell.x + 2, cell.y));
        if (cell.y > 1 && maze[cell.x, cell.y - 2] == 0) neighbors.Add(new Vector2Int(cell.x, cell.y - 2));
        if (cell.y < height - 2 && maze[cell.x, cell.y + 2] == 0) neighbors.Add(new Vector2Int(cell.x, cell.y + 2));

        return neighbors;
    }

    void DrawMaze()
    {
        // Clear previous maze
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the floor
        GameObject floor = Instantiate(floorPrefab, new Vector3(width / 2f - 0.5f, -0.5f, height / 2f - 0.5f), Quaternion.identity, transform);
        floor.transform.localScale = new Vector3(width / 10f, 1, height / 10f);

        // Draw the walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 0)
                {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                }
                else
                {
                    Debug.Log($"Path cell at ({x}, {y})");
                }
            }
        }

        // Draw the edge walls
        for (int x = -1; x <= width; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, -1), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(x, 0, height), Quaternion.identity, transform);
        }
        for (int y = -1; y <= height; y++)
        {
            Instantiate(wallPrefab, new Vector3(-1, 0, y), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(width, 0, y), Quaternion.identity, transform);
        }
    }

    void PlaceStartAndGoal()
    {
        // Ensure the start and goal are placed on path cells
        if (maze[startCell.x, startCell.y] == 1)
        {
            Instantiate(startPrefab, new Vector3(startCell.x, 0, startCell.y), Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning("Start position is not on a path cell.");
        }

        if (maze[goalCell.x, goalCell.y] == 1)
        {
            Instantiate(goalPrefab, new Vector3(goalCell.x, 0, goalCell.y), Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning("Goal position is not on a path cell.");
        }
    }

    void PlaceCoins()
    {
        int numberOfCoins = currentLevel;
        List<Vector2Int> pathCells = new List<Vector2Int>();

        // Collect all path cells
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    pathCells.Add(new Vector2Int(x, y));
                }
            }
        }

        // Shuffle path cells
        for (int i = 0; i < pathCells.Count; i++)
        {
            Vector2Int temp = pathCells[i];
            int randomIndex = rand.Next(i, pathCells.Count);
            pathCells[i] = pathCells[randomIndex];
            pathCells[randomIndex] = temp;
        }

        // Place coins on random path cells
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector2Int coinCell = pathCells[i];
            Instantiate(coinPrefab, new Vector3(coinCell.x, 0.5f, coinCell.y), Quaternion.identity, transform);
        }
    }

    Vector2Int FindGoalCell()
    {
        // Start searching from the top-right corner towards the start
        for (int x = width - 1; x >= 0; x--)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                if (maze[x, y] == 1)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        // If no valid cell found, return the bottom-left corner
        return new Vector2Int(0, 0);
    }

    public void GenerateNextLevel()
    {
        // Destroy current player instance
        Destroy(currentPlayer);

        // Increase maze size
        width += 2;
        height += 2;
        currentLevel++;

        // Generate new maze
        GenerateMaze();
        DrawMaze();

        // Place start and goal
        PlaceStartAndGoal();

        // Place coins
        PlaceCoins();

        // Spawn new player instance
        SpawnPlayer();

        // Adjust Camera
        cameraController.FitCameraToMaze();
    }

    void SpawnPlayer()
    {
        // Instantiate player prefab at starting position
        currentPlayer = Instantiate(playerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
}
