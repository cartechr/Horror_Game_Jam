using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    //Is the player currently movable?
    [SerializeField] bool PlayerActive = true;

    Item_Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Item_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //if both items are picked up, you can scroll between them
        if (inventory.Slot2_Active)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                //Switch to Flashlight
                if (inventory.Active == "Pipe")
                {
                    inventory.Active = "Flashlight";

                    //Objects
                    inventory.pipe_Hand.SetActive(false);
                    inventory.flashlight_Hand.SetActive(true);

                    //UI
                    inventory.one_Pipe.SetActive(false);
                    inventory.two_flashlight.SetActive(false);

                    inventory.two_Pipe.SetActive(true);
                    inventory.one_flashlight.SetActive(true);
                }
                //Switch to Pipe
                else if (inventory.Active == "Flashlight")
                {
                    inventory.Active = "Pipe";

                    //Objects
                    inventory.pipe_Hand.SetActive(true);
                    inventory.flashlight_Hand.SetActive(false);

                    //UI
                    inventory.one_Pipe.SetActive(true);
                    inventory.two_flashlight.SetActive(true);

                    inventory.two_Pipe.SetActive(false);
                    inventory.one_flashlight.SetActive(false);
                }
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                //Switch to Flashlight
                if (inventory.Active == "Pipe")
                {
                    inventory.Active = "Flashlight";

                    //Objects
                    inventory.pipe_Hand.SetActive(false);
                    inventory.flashlight_Hand.SetActive(true);

                    //UI
                    inventory.one_Pipe.SetActive(false);
                    inventory.two_flashlight.SetActive(false);

                    inventory.two_Pipe.SetActive(true);
                    inventory.one_flashlight.SetActive(true);
                }
                //Switch to Pipe
                else if (inventory.Active == "Flashlight")
                {
                    inventory.Active = "Pipe";

                    //Objects
                    inventory.pipe_Hand.SetActive(true);
                    inventory.flashlight_Hand.SetActive(false);

                    //UI
                    inventory.one_Pipe.SetActive(true);
                    inventory.two_flashlight.SetActive(true);

                    inventory.two_Pipe.SetActive(false);
                    inventory.one_flashlight.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Most recent picked up item becomes the active item
        if (other.gameObject.CompareTag("Flashlight"))
        {

            Debug.Log("Pick Up Item");
            if (Input.GetKey (KeyCode.E))
            {
                if (!inventory.Slot1_Active)
                {
                    inventory.Slot1_Active = true;
                    inventory.Slot1_gameObject = "Flashlight";
                    inventory.Active = "Flashlight";
                    other.gameObject.SetActive(false);

                    //Objects
                    inventory.flashlight_Hand.SetActive(true);

                    //UI
                    inventory.one_flashlight.SetActive(true);
                }
                else
                {
                    inventory.Slot2_Active = true;
                    inventory.Slot2_gameObject = "Flashlight";
                    inventory.Active = "Flashlight";
                    other.gameObject.SetActive(false);

                    //Objects
                    inventory.pipe_Hand.SetActive(false);
                    inventory.flashlight_Hand.SetActive(true);

                    //UI
                    inventory.one_Pipe.SetActive(false);
                    inventory.one_flashlight.SetActive(true);
                    inventory.two_Pipe.SetActive(true);
                }
            }
        }
        if (other.gameObject.CompareTag("Pipe"))
        {
            Debug.Log("Pick Up Item");
            if (Input.GetKey (KeyCode.E))
            {
                if (!inventory.Slot1_Active)
                {
                    inventory.Slot1_Active = true;
                    inventory.Slot1_gameObject = "Pipe";
                    inventory.Active = "Pipe";
                    other.gameObject.SetActive(false);

                    //Objects
                    inventory.pipe_Hand.SetActive(true);

                    //UI
                    inventory.one_Pipe.SetActive (true);
                }
                else
                {
                    inventory.Slot2_Active = true;
                    inventory.Slot2_gameObject = "Pipe";
                    inventory.Active = "Pipe";
                    other.gameObject.SetActive(false);

                    //Objects
                    inventory.flashlight_Hand.SetActive(false);
                    inventory.pipe_Hand.SetActive(true);

                    //UI
                    inventory.one_flashlight.SetActive (false); 
                    inventory.two_flashlight.SetActive (true);
                    inventory.one_Pipe.SetActive(true);
                }
            }
        }
    }
}
