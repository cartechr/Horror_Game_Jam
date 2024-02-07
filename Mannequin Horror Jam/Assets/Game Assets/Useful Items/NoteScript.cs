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
    [Tooltip("Assign Note Image here")]
    [SerializeField] Sprite noteImageSprite;
    [Tooltip("Assign Note Image UI Component Here")]
    [SerializeField] Image noteImageUIComponent;
    [Tooltip("Assign Relevant Note UI element withing player to here")]
    [SerializeField] TextMeshProUGUI noteTextUI;

    [Tooltip("Assign FPS Control Here")]
    public FPSCONTROL fpsControl;

    bool noteIsOpen = false;

    private void Start()
    {
        fpsControl = GameObject.FindWithTag("Player").GetComponent<FPSCONTROL>();
    }

    private void Update()
    {

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
        fpsControl.animator.SetBool("isInteracting", true);
        fpsControl.disableLook = true;
        fpsControl.disableMovement = true;
        noteUI.SetActive(true);
        noteImageUIComponent.sprite = noteImageSprite;
        noteTextUI.text = enterText;
        noteIsOpen = true;
        Debug.Log("noteIsOpen " + noteIsOpen);

    }

    void CloseNote()
    {
        fpsControl.animator.SetBool("isInteracting", false);
        fpsControl.disableLook = false;
        fpsControl.disableMovement = false;
        noteUI.SetActive(false);
        noteIsOpen = false;
        Debug.Log("noteIsOpen " + noteIsOpen);

    }

}
