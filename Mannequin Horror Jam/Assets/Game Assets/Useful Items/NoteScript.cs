using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteScript : MonoBehaviour
{
    [Header("Note Assignments")]
    [Tooltip("Enter text here for player to read")]
    [TextArea(3, 10)]
    [SerializeField] public string enterText;
    [Tooltip("Assign Relevant Note UI within player to here")]
    [SerializeField] GameObject noteUI;
    [Tooltip("Assign Relevant Note UI element withing player to here")]
    [SerializeField] TextMeshProUGUI noteTextUI;

    FPSCONTROL fpsControl;

    bool noteIsOpen = false;

    private void Start()
    {
        fpsControl = GameObject.FindWithTag("Player").GetComponent<FPSCONTROL>();
    }

    public void ToggleNote()
    {
        if(!noteIsOpen)
        {
            OpenNote();
        }
        else if(noteIsOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)))
        {
            CloseNote();
        }

    }

    void OpenNote()
    {
        fpsControl.disableLook = true;
        fpsControl.disableMovement = true;
        noteUI.SetActive(true);
        noteTextUI.text = enterText;
        noteIsOpen = true;
        Debug.Log("noteIsOpen " + noteIsOpen);

    }

    void CloseNote()
    {
        fpsControl.disableLook = false;
        fpsControl.disableMovement = false;
        noteUI.SetActive(false);
        noteIsOpen = false;
        Debug.Log("noteIsOpen " + noteIsOpen);

    }

}
