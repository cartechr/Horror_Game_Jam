using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    [Header("Door Control Variables")]
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openSpeed = 2f;

    [Header("Lock Controls")]
    [Tooltip("Check if you want the door to be locked")]
    [SerializeField] public bool isLocked = false;

    [Header("Choose which key is required to open the door")]
    public bool requireRedKey;
    public bool requireBlueKey;
    public bool requireGreenKey;
    public bool requireBlackKey;

    [Header("Dialogue Controls")]
    [Tooltip("Assign the relevant UI element")]
    [SerializeField] GameObject commentaryPanel;
    [Tooltip("Assign the relevant UI text element")]
    [SerializeField] TextMeshProUGUI commentaryText;
    [Tooltip("Enter a text to show if the door is locked and there is no key")]
    [SerializeField] string doorLocked = "Door is locked!";
    [Tooltip("Enter a text to show if the door is unlocked")]
    [SerializeField] string doorUnlocked = "Door is unlocked!";

    [Tooltip("Assign relevant script")]
    [SerializeField] BasicInventory inventory;
    [SerializeField] FMODEvents fmodEvents;

    private bool isOpening = false;
    private Quaternion initialRotation;

    [field: Header("FMOD Related SFX")]
    [field: SerializeField] public EventReference doorCreak { get; private set; }
    public EventInstance doorCreakInst;
    [field: SerializeField] public EventReference doorRattle { get; private set; }
    public EventInstance doorRattleInst;
    [field: SerializeField] public EventReference doorUnlock { get; private set; }
    public EventInstance doorUnlockInst;
    [field: SerializeField] public EventReference metalDoorClose { get; private set; }
    public EventInstance metalDoorCloseInst;
    [field: SerializeField] public EventReference metalDoorOpen { get; private set; }
    public EventInstance metalDoorOpenInst;
    [field: SerializeField] public EventReference sarahRoomMusic { get; private set; }
    public EventInstance sarahRoomMusicInst;

    private void Start()
    {
        // Store the initial rotation of the door
        initialRotation = transform.rotation;

        GameObject player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<BasicInventory>();

        GameObject fmodEventsObj = GameObject.FindWithTag("FMODEvents");
        fmodEvents = fmodEventsObj.GetComponent<FMODEvents>();
        
    }

    void TryGetComponent()
    {
        if(inventory == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            inventory = player.GetComponent<BasicInventory>();
        }
    }

    private void Update()
    {
        if (isOpening)
        {
            // Use Mathf.Lerp to smoothly rotate the door towards the open position
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation * Quaternion.Euler(0, openAngle, 0), openSpeed * Time.deltaTime);

        }
        else
        {
            // Close the door
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, openSpeed * Time.deltaTime);
        }

        TryGetComponent();

    }

    public void PlaySFXOnLocation(EventInstance eventInstance, EventReference eventReference)
    {
        
        eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        eventInstance.start();
        eventInstance.release();
    }


    public void ToggleDoor()
    {

        if (!isLocked)
        {
            fmodEvents.startFX(doorCreakInst, doorCreak);

            AudioManager.instance.PlayOneShot(FMODEvents.instance.doorCreak, this.transform.position);

            // Toggle the door state (open/close)
            isOpening = !isOpening;
        }
    }

    public void DoorLockControl()
    {
        if (isLocked && requireRedKey)
        {
            if (inventory.hasRedKey)
            {
                isLocked = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.doorUnlock, this.transform.position); //SFX
                Debug.Log("isLocked " + isLocked);
                inventory.hasRedKey = false;
                Debug.Log("Has key?" + inventory.hasRedKey);
                StartCoroutine(DoorUnlockedInfo());
            }
        }

        if (isLocked && requireGreenKey)
        {
            if (inventory.hasGreenKey)
            {
                isLocked = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.doorUnlock, this.transform.position); //SFX
                Debug.Log("isLocked " + isLocked);
                inventory.hasGreenKey = false;
                Debug.Log("Has key?" + inventory.hasGreenKey);
                StartCoroutine(DoorUnlockedInfo());
            }
        }

        if (isLocked && requireBlueKey)
        {

            if (inventory.hasBlueKey)
            {
                isLocked = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.doorUnlock, this.transform.position); //SFX
                Debug.Log("isLocked " + isLocked);
                inventory.hasBlueKey = false;
                Debug.Log("Has key?" + inventory.hasBlueKey);
                StartCoroutine(DoorUnlockedInfo());
            }
        }

        if (isLocked && requireBlackKey)
        {
            if (inventory.hasBlackKey)
            {
                isLocked = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.doorUnlock, this.transform.position); //SFX
                Debug.Log("isLocked " + isLocked);
                inventory.hasBlackKey = false;
                Debug.Log("Has key?" + inventory.hasBlackKey);
                StartCoroutine(DoorUnlockedInfo());
            }
        }
    }

    public void LockedDialogue()
    {
        StartCoroutine(DoorLockedInfo());
    }


    IEnumerator DoorLockedInfo()
    {
        Debug.Log("Door Locked Coroutine Started");
        commentaryPanel.SetActive(true);
        commentaryText.text = doorLocked;
        //fmodEvents.startFX(doorRattleInst, doorRattle); //FMOD SFX
        //PlaySFXOnLocation(doorRattleInst, doorRattle); //FMOD FX

        AudioManager.instance.PlayOneShot(FMODEvents.instance.doorRattle, this.transform.position); //SFX

        yield return new WaitForSeconds(2f);
        commentaryPanel.SetActive(false);
    }

    IEnumerator DoorUnlockedInfo()
    {
        Debug.Log("Door Unlock Coroutine Started");
        commentaryPanel.SetActive(true);
        commentaryText.text = doorUnlocked;

        //fmodEvents.startFX(doorUnlockInst, doorUnlock); //FMOD SFX

        //fmodEvents.parameterChange(sarahRoomMusicInst, "PickupObject", "True");

        sarahRoomMusicInst = FMODUnity.RuntimeManager.CreateInstance(sarahRoomMusic);
        sarahRoomMusicInst.setParameterByNameWithLabel("PickupObject", "True");

        yield return new WaitForSeconds(2f);
        commentaryPanel.SetActive(false);
    }



}
