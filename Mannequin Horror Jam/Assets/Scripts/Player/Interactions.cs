using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Interactions : MonoBehaviour
{
    Inventory_Item inventory;
    public Inventory_Manager inventoryManager;

    StarterAssetsInput starterAssetsInput;
    public GameObject FullInv;

    FirstPersonControll FirstPersonController;

    [Tooltip ("Script is attached to 'Full Inv' inventory gameobject")]
    public UI_Buttons ui_buttons;

    private void Start()
    {
        inventory = GetComponent<Inventory_Item>();
        starterAssetsInput = GetComponent<StarterAssetsInput>();
        FirstPersonController = GetComponent<FirstPersonControll>();
    }

    private void Update()
    {
        Items item = Inventory_Manager.Instance.GetSelectedItem();

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            int newSlot = inventoryManager.selectedSlot + (scroll > 0 ? 1 : -1);
            Debug.Log(item);

            if (newSlot < 0)
            {
                newSlot = inventoryManager.inventorySlots.Length - 1;
            }
            else if (newSlot >= inventoryManager.inventorySlots.Length)
            {
                newSlot = 0;
            }

            StartCoroutine(SwitchSlotWithDelay(newSlot, 0.0f));
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Item Used");
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!FirstPersonController.InvOpen)
            {
                //Disable player movement when Inv opened
                Debug.Log("Inv Open");
                FirstPersonController.InvOpen = true;
                FullInv.SetActive(true);

                //Mouse
                starterAssetsInput.cursorLocked = false;
                starterAssetsInput.cursorInputForLook = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                //Items Inventory Tab should alway open first
                ui_buttons.num = 2;
            }
            else
            {
                //Enable player movement when Inv closed
                Debug.Log("Inv Closed");
                FirstPersonController.InvOpen = false;
                FullInv.SetActive(false);

                //Mouse
                starterAssetsInput.cursorLocked = true;
                starterAssetsInput.cursorInputForLook = true;
                Cursor.visible = false;
                Cursor.lockState= CursorLockMode.Locked;
            }
        }
    }

    private IEnumerator SwitchSlotWithDelay(int newSlot, float delay)
    {
        yield return new WaitForSeconds(delay);
        newSlot = Mathf.Clamp(newSlot, 0, inventoryManager.inventorySlots.Length - 1);
        inventoryManager.ChangeSelectedSlot(newSlot);
    }
}
