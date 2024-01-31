using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openSpeed = 2f;
    [SerializeField] bool isLocked = false;

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
    }

    public void ToggleDoor()
    {
        if (!isLocked || (isLocked && inventory.hasKey))
        {
            // Toggle the door state (open/close)
            isOpening = !isOpening;
        }
        else
        {
            Debug.Log("Door is locked, find a key");
        }
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}
