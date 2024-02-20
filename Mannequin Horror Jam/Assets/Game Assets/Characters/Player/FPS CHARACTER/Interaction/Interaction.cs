using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    [Header("Assignments")]
    [SerializeField] BasicInventory inventory;
    [SerializeField] FPSCONTROL fpsControl;
    [SerializeField] GameObject flashlight;
    [SerializeField] GameObject walkieTalkieObj;

    [Header("Interaction Variables")]
    [SerializeField] float interactionDistance = 3f;
    [SerializeField] bool rayTouchCheck;

    [Header("UI Stuff")]
    [SerializeField] GameObject nonActiveCursor;
    [SerializeField] GameObject activeCursor;

    CommentaryScript commentaryScript;
    NextScene nextScene;

    GameObject fmodObject;
    FMODEvents fmodEvents;

    private void Start()
    {
        inventory = GetComponent<BasicInventory>();

        fmodObject = GameObject.FindGameObjectWithTag("Player");
        fmodEvents = fmodObject.GetComponent< FMODEvents>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            DoorInteract();
            NoteInteraction();
            DialogueInteractions();
            FlashlightInteraction();
            WalkieTalkieInteraction();
            SceneTransition();
        }

        hitChecker();

    }

    public void hitChecker()
    {

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
           out RaycastHit hit, interactionDistance))
        {

            if(hit.collider.CompareTag("Door") || hit.collider.CompareTag("Note") || hit.collider.CompareTag("Key") 
                || hit.collider.CompareTag("Drawer") || hit.collider.CompareTag("Flashlight") || hit.collider.CompareTag("WalkieTalkie"))
            {
                nonActiveCursor.SetActive(false);
                activeCursor.SetActive(true);
            }
            else
            {
                nonActiveCursor.SetActive(true);
                activeCursor.SetActive(false);
            }

        }
    }

    public void SceneTransition()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
           out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("SceneTransition"))
            {

                var nextScene = hit.collider.GetComponent<NextScene>();

                nextScene.LoadScene();

                if (nextScene.name == "Apartment Complex - LEVEL 03")
                {
                    fmodEvents.stopFadeSarah();
                }

            }
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
                    if (!doorScript.isLocked)
                    {
                        doorScript.ToggleDoor();
                    }
                    if (doorScript.isLocked && doorScript.requireRedKey && inventory.hasRedKey) 
                    {
                        doorScript.DoorLockControl();
                    }

                    if(doorScript.isLocked && doorScript.requireBlackKey && inventory.hasBlackKey)
                    {
                        doorScript.DoorLockControl();
                    }

                    if(doorScript.isLocked && doorScript.requireBlueKey && inventory.hasBlueKey)
                    {
                        doorScript.DoorLockControl();
                    }

                    if(doorScript.isLocked && doorScript.requireGreenKey && inventory.hasGreenKey)
                    {
                        doorScript.DoorLockControl();
                    }

                    else if (doorScript.isLocked)
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

    void FlashlightInteraction()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
           out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Flashlight"))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.flashlightPickup, this.transform.position);
                GameObject flashLight = hit.collider.gameObject;
                inventory.hasFlashlight = true;
                Destroy(flashLight);
                flashlight.SetActive(true);

            }
        }
    }

    void WalkieTalkieInteraction()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
           out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("WalkieTalkie"))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.walkiePickup, this.transform.position);
                GameObject walkieTalkie = hit.collider.gameObject;
                inventory.hasWalkieTalkie = true;
                Destroy(walkieTalkie);
                walkieTalkieObj.SetActive(true);
            }
        }
    }

}
