using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInventory : MonoBehaviour
{
    [Header("Section for Items")]
    [SerializeField] GameObject[] items;

    [Header("Section for Keys")]
    [SerializeField] public GameObject[] keys;
    [SerializeField] public GameObject key;

    [Header("Bool Checks")]
    [SerializeField] public bool hasKey;

    [Header("Interaction Settings")]
    [SerializeField] public float interactionDistance = 3f;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward *
                interactionDistance, out RaycastHit hit, 3f))
            {
                if (hit.collider.CompareTag("Key"))
                {
                    key = hit.collider.gameObject;
                    Debug.Log("Ray Hitting Key");
                    //Toggle Door opening and closing
                    PickupKey();
                }
            }
        }
    }


    public void PickupKey()
    {

        //Add the key into the array

        /*
        key = GameObject.FindWithTag("Key");
        hasKey = true;
        Object.Destroy(key);
        */
    }


}
