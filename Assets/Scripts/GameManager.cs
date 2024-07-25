using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }

        // Check if the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartScene();
        }
    }

    void QuitApplication()
    {
        Application.Quit();
    }

    void RestartScene()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }
}
