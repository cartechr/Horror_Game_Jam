using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{

    [SerializeField] GameObject uiPanel;
    bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            uiPanel.SetActive(true);
            PauseTheGame();
        }
        else
        {
            uiPanel.SetActive(false);
            ResumeGame();
        }
    }

    void PauseTheGame()
    {
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None; // Unlock mouse
        Cursor.visible = true; // Make mouse visible
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock mouse
        Cursor.visible = false; // Make mouse invisible
    }

}
