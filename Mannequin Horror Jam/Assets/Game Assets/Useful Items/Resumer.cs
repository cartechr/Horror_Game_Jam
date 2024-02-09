using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resumer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }

    public void HideCursor()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse
        Cursor.visible = false; // Make the mouse invisible
    }
}
