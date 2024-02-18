using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{

    [SerializeField] GameObject uiPanel;
    bool isPaused = false;

    public GameObject Player;
    public GameObject DeadUI;
    public GameObject SpamUI;


    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Player.GetComponent<FPSCONTROL>().playerDead)
            {
                TogglePause();
            }
        }


       /* if (!switchMusic)
        {
            switchMusic = true;
            fmodEvents.startHallWay(Player);
        }*/

        Dead_UI();
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

    void Dead_UI()
    {
        if (Player.GetComponent<FPSCONTROL>().playerDead)
        {
            DeadUI.gameObject.SetActive(true);
            SpamUI.gameObject.SetActive(false);
            PauseTheGame();
            Cursor.visible = true;
            Debug.Log("Player Dead");
            Player.GetComponent<FPSCONTROL>().Health = 3;
        }

        Player.GetComponent<FPSCONTROL>().playerDead = false;
    }

}
