using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicInventory : MonoBehaviour
{
    [Header("UI Section Assign Relevant UI Elements")]
    [Tooltip("Assign the relevant UI element")]
    public GameObject keysPanel; //Disabling and enabling can be complicated instead make the background transparent
    public GameObject redKeyUI;
    public GameObject blueKeyUI;
    public GameObject greenKeyUI;
    public GameObject blackKeyUI;


    [Header("Section for Items")]
    [SerializeField] List<GameObject> items = new List<GameObject>();

    [Header("Section for Keys")]
    [SerializeField] List<GameObject> keys = new List<GameObject>();

    [Header("Bool Checks")]
    [SerializeField] public bool hasBlackKey;
    [SerializeField] public bool hasRedKey;
    [SerializeField] public bool hasGreenKey;
    [SerializeField] public bool hasBlueKey;

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
                    GameObject key = hit.collider.gameObject;
                    Debug.Log("Ray Hitting Key");
                    
                    PickupKey(key);
                }
            }
        }

        ControlUI();

    }


    public void PickupKey(GameObject key)
    {

        keys.Add(key);

        if(key.transform.name == "Key_Red")
        {
            hasRedKey = true;
            Object.Destroy(key);
        }

        if (key.transform.name == "Key_Blue")
        {
            hasBlueKey = true;
            Object.Destroy(key);
        }

        if (key.transform.name == "Key_Green")
        {
            hasGreenKey = true;
            Object.Destroy(key);
        }

        if (key.transform.name == "Key_Black")
        {
            hasBlackKey = true;
            Object.Destroy(key);
        }

    }

    void ControlUI()
    {
        if(hasRedKey)
        {
            redKeyUI.SetActive(true);

        }
        else
        {
            redKeyUI.SetActive(false);
        }

        if (hasBlueKey)
        {
            blueKeyUI.SetActive(true);
        }
        else
        {
            blueKeyUI.SetActive(false);
        }

        if (hasGreenKey)
        {
            greenKeyUI.SetActive(true);
        }
        else
        {
            greenKeyUI.SetActive(false);
        }

        if (hasBlackKey)
        {
            blackKeyUI.SetActive(true);
        }
        else
        {
            blackKeyUI.SetActive(false);
        }


    }

}
