using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] float interactionDistance = 3f;
    [SerializeField] BasicInventory inventory;

    private void Start()
    {
        inventory = GetComponent<BasicInventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
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
                        Debug.Log("Door is locked, find a key");
                    }
                }
            }
        }
    }
}
