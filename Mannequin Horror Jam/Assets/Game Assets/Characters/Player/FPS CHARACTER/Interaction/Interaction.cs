using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] float interactionDistance = 3f;
    [SerializeField] BasicInventory inventory;
    [SerializeField] FPSCONTROL fpsControl;

    CommentaryScript commentaryScript;

    private void Start()
    {
        inventory = GetComponent<BasicInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            DoorInteract();
            NoteInteraction();
            DialogueInteractions();
        }
    }

    private void DoorInteract()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 
            out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                var doorScript = hit.collider.GetComponent<DoorScript>();

                if (doorScript != null)
                {
                    if (!doorScript.IsLocked() || (doorScript.IsLocked() && inventory.hasKey))
                    {
                        doorScript.ToggleDoor();
                    }
                    else
                    {
                        doorScript.LockedDialogue();
                    }
                }
            }
        }
    }


    void NoteInteraction()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
           out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Note"))
            {
                var noteScript = hit.collider.GetComponent<NoteScript>();

                if(noteScript != null)
                {
                    noteScript.ToggleNote();
                    Debug.Log("Trying to interact with the note");
                }

            }
        }
    }

    void DialogueInteractions()
    {

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
           out RaycastHit hit, interactionDistance))
        {

            var dialogueScript = hit.collider.GetComponent<CommentaryScript>();

            if (dialogueScript != null && !dialogueScript.inDialogue)
            {
                dialogueScript.DialogueStarter();
            }
        }
    }


}
