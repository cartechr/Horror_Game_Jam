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

    [SerializeField] GameObject eventsObject;
    [SerializeField] FMODEvents fmodEvents;

    private bool isOpening = false;
    private Quaternion initialRotation;

    private void Start()
    {
        //fmodEvents.startFX(fmodEvents.sarahRoomMusicInst, fmodEvents.sarahRoomMusic, eventsObject);
        // Store the initial rotation of the door
        initialRotation = transform.rotation;

        GameObject player = GameObject.FindWithTag("Player");


        eventsObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = eventsObject.GetComponent< FMODEvents>();
        
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

    public void ToggleDoor()
    {

        if (!isLocked)
        {
            //fmodEvents.playSFXOnLocation(fmodEvents.doorCreakInst, fmodEvents.doorCreak, this.gameObject);

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
        //fmodEvents.playSFXOnLocation(fmodEvents.doorRattleInst, fmodEvents.doorRattle, this.gameObject); //FMOD FX


        yield return new WaitForSeconds(2f);
        commentaryPanel.SetActive(false);
    }

    IEnumerator DoorUnlockedInfo()
    {
        Debug.Log("Door Unlock Coroutine Started");
        commentaryPanel.SetActive(true);
        commentaryText.text = doorUnlocked;

        //fmodEvents.startFX(fmodEvents.doorUnlockInst, fmodEvents.doorUnlock); //FMOD SFX

        //fmodEvents.playSFXOnLocation(fmodEvents.doorUnlockInst, fmodEvents.doorUnlock, this.gameObject);

        //fmodEvents.stopImmediateFX(fmodEvents.sarahRoomMusicInst);

        //fmodEvents.StopSarah();
        fmodEvents.changeSarah();
        //fmodEvents.parameterChange(fmodEvents.sarahRoomMusicInst, "PickupObject", "True");

        //sarahRoomMusicInst = FMODUnity.RuntimeManager.CreateInstance(sarahRoomMusic);
       // sarahRoomMusicInst.setParameterByNameWithLabel("PickupObject", "True");

        yield return new WaitForSeconds(2f);
        commentaryPanel.SetActive(false);
    }


    [field: Header("Music")]
    [field: SerializeField] public EventReference sarahRoomMusic { get; private set; }
    public EventInstance sarahRoomMusicInst;
    [field: SerializeField] public EventReference hallwayMusic { get; private set; }
    public EventInstance hallwayMusicInst;


    [field: Header("Ambiance")]
    [field: SerializeField] public EventReference stairWellAmbiance { get; private set; }
    public EventInstance stairWellAmbianceInst;
    [field: SerializeField] public EventReference rainAmbiance { get; private set; }
    public EventInstance rainAmbianceInst;


    [field: Header("Item & Interaction")]
    [field: SerializeField] public EventReference uiInteractSFX { get; private set; }
    public EventInstance uiInteractSFXInst;
    [field: SerializeField] public EventReference keyPickup { get; private set; }
    public EventInstance keyPickupInst;
    [field: SerializeField] public EventReference notePickup { get; private set; }
    public EventInstance notePickupInst;
    [field: SerializeField] public EventReference doorCreak { get; private set; }
    public EventInstance doorCreakInst;
    [field: SerializeField] public EventReference doorRattle { get; private set; }
    public EventInstance doorRattleInst;
    [field: SerializeField] public EventReference doorUnlock { get; private set; }
    public EventInstance doorUnlockInst;
    [field: SerializeField] public EventReference floorCreak { get; private set; }
    public EventInstance floorCreakInst;
    [field: SerializeField] public EventReference metalDoorClose { get; private set; }
    public EventInstance metalDoorCloseInst;
    [field: SerializeField] public EventReference metalDoorOpen { get; private set; }
    public EventInstance metalDoorOpenInst;


    [field: Header("Footstep Specific")]
    [field: SerializeField] public EventReference footStep { get; private set; }
    public EventInstance footStepInst;

    [field: Header("Clothes SFX")]
    [field: SerializeField] public EventReference clothesRustling { get; private set; }
    public EventInstance clothesRustlingInst;

    [field: Header("Mannequins")]
    [field: SerializeField] public EventReference headTurns { get; private set; }
    public EventInstance headTurnInst;
    [field: SerializeField] public EventReference mannequinMovement { get; private set; }
    public EventInstance mannequinMovementInst;
    [field: SerializeField] public EventReference maskRattle { get; private set; }
    public EventInstance maskRattleInst;

    [field: Header("Flashlight")]
    [field: SerializeField] public EventReference flashlightClick { get; private set; }
    public EventInstance flashlightClickInst;
    [field: SerializeField] public EventReference flashlightPickup { get; private set; }
    public EventInstance flashlightPickupInst;

    [field: Header("Walkie-Talkie")]
    [field: SerializeField] public EventReference walkiePickup { get; private set; }
    public EventInstance walkiePickupInst;

    [field: Header("Damage Indicator")]
    [field: SerializeField] public EventReference takingDamage { get; private set; }
    public EventInstance takingDamageInst;

}
