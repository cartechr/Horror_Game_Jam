using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInventory : MonoBehaviour
{

    [SerializeField] GameObject[] items;

    [SerializeField] public bool hasKey;
    [SerializeField] public GameObject key;
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
                    Debug.Log("Ray Hitting Key");
                    //Toggle Door opening and closing
                    PickupKey();
                }
            }
        }
    }


    public void PickupKey()
    {
        key = GameObject.FindWithTag("Key");
        hasKey = true;
        Object.Destroy(key);
    }


}
