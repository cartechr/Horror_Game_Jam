using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    [SerializeField] string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
