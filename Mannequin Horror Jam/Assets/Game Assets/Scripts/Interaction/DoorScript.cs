using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    [SerializeField] Transform playerCamera;
    [SerializeField] float interactionDistance = 3f;
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openSpeed = 2f;
    //[SerializeField] Reference to Inventory System

    [SerializeField] bool isLocked = false;


    bool isOpening = false;
    
    Quaternion initialRotation;


    private void Start()
    {
        //Store the initial rotation of the door
        initialRotation = transform.rotation;


    }

    private void Update()
    {
        if (!isLocked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * interactionDistance, out RaycastHit hit, 3f))
                {
                    if (hit.collider.CompareTag("Door"))
                    {
                        Debug.Log("Ray hitting door");
                        //Toggle Door opening and closing
                        ToggleDoor();
                    }
                }
            }

            if (isOpening)
            {
                // Use Mathf.Lerp to smoothly rotate the door towards the open position
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation *
                    Quaternion.Euler(0, openAngle, 0), openSpeed * Time.deltaTime);
            }
            else
            {
                // Close the door
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, openSpeed * Time.deltaTime);

            }
        }

        if (isLocked) // && Inventory script has key
        {

        }


    }

    private void ToggleDoor()
    {
        // Toggle the door state (open/close)
        isOpening = !isOpening;
    }

    private void OnDrawGizmos()
    {
        // Draw a line from the camera aiming straight forward
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactionDistance);

    }

}


