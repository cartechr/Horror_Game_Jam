using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openSpeed = 2f;
    [SerializeField] bool isLocked = false;

    [SerializeField] GameObject commentaryPanel;
    [SerializeField] TextMeshProUGUI commentaryText;
    [SerializeField] string doorLocked = "Door is locked!";
    [SerializeField] string doorUnlocked = "Door is unlocked!";

    [SerializeField] BasicInventory inventory;

    private bool isOpening = false;
    private Quaternion initialRotation;

    private void Start()
    {
        // Store the initial rotation of the door
        initialRotation = transform.rotation;
        GameObject player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<BasicInventory>();
        
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
        if (isLocked)
        {
            if (inventory.hasKey)
            {
                isLocked = false;
                Debug.Log("isLocked " + isLocked);
                inventory.hasKey = false;
                Debug.Log("Has key?" + inventory.hasKey);
                StartCoroutine(DoorUnlockedInfo());
            }
        }

        if (!isLocked)
        {
            // Toggle the door state (open/close)
            isOpening = !isOpening;
        }
        else
        {
            StartCoroutine(DoorLockedInfo());

        }
    }

    public bool IsLocked()
    {
        return isLocked;
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
        yield return new WaitForSeconds(2f);
        commentaryPanel.SetActive(false);
    }

    IEnumerator DoorUnlockedInfo()
    {
        Debug.Log("Door Unlock Coroutine Started");
        commentaryPanel.SetActive(true);
        commentaryText.text = doorUnlocked;
        yield return new WaitForSeconds(2f);
        commentaryPanel.SetActive(false);
    }

}
