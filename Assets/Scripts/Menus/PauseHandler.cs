using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    File: PauseHandler.cs 
    Author: Matthew McFarland
    Date: 12/1/2022

    Handles pausing
**/

public class PauseHandler : MonoBehaviour
{
    public GameObject pausePanel;
    public bool isPaused = false;

    private float prevTimeScale = 1f;

    public void PauseGame()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        // Stop Player Movement
    }

    public void ResumeGame()
    {
        Time.timeScale = prevTimeScale;
        // Resume Player Movement
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            pausePanel.SetActive(true);
            PauseGame();
        }
        else
        {
            pausePanel.SetActive(false);
            ResumeGame();
        }
    }
}
