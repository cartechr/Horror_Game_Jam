using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{

    [SerializeField] string nextSceneName;
    public GameObject toggle;

    float Fade = 255;
    float fadeDamage = 40;
    float endFade = 0;
    bool startFade;

    public GameObject fmodObject;
    public FMODEvents fmodEvents;

    private void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName);
        Time.timeScale = 1.0f;
    }

    public void playGame()
    {
        startFade = true;
        toggle.SetActive(true);

        fmodEvents.startGame();
    }

    private void Update()
    {
        if (startFade)
        {
            if (endFade < Fade)
            {
                endFade += fadeDamage * Time.deltaTime;
                Debug.Log(endFade);
                toggle.GetComponent<Image>().color = new Color32(0, 0, 0, Convert.ToByte(endFade));
            }
            else
            {
                Debug.Log("Play game here");
                SceneManager.LoadScene(1);
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void Restart()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        Time.timeScale = 1.0f;
    }

    public void TerminateProgram()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
