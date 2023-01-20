using System;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    [SerializeField] private GameObject pauseScreen;
    public static bool isGamePaused;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    
    private void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }
}