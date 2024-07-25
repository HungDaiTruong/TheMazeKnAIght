using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private MazeGenerator mazeGenerator;

    void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        mainCamera = Camera.main;
        mazeGenerator = FindObjectOfType<MazeGenerator>();

        if (mainCamera != null && mazeGenerator != null)
        {
            FitCameraToMaze();
        }
        else
        {
            Debug.LogWarning("Main camera or MazeGenerator not found.");
        }
    }

    public void FitCameraToMaze()
    {
        // Calculate the size of the maze based on its width and height
        float mazeWidth = mazeGenerator.width;
        float mazeHeight = mazeGenerator.height;

        // Calculate the aspect ratio of the maze
        float mazeAspectRatio = mazeWidth / mazeHeight;

        // Orthographic size is half of the height of the view
        float targetOrthoSize = mazeHeight;

        // Adjust camera position to center on the maze
        Vector3 targetPosition = new Vector3(mazeWidth / 2f - 0.5f, targetOrthoSize, mazeHeight / 2f - 0.5f);

        // Set camera properties
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetOrthoSize;
    }
}
