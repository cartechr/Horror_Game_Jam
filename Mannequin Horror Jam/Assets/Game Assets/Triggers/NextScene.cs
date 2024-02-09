using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    [SerializeField] string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName);
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
