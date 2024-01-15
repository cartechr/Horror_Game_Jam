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

    private void Start()
    {
        inventory = GetComponent<Inventory_Item>();
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
            Debug.Log("Just Do It Bro");
        }
    }

    private IEnumerator SwitchSlotWithDelay(int newSlot, float delay)
    {
        yield return new WaitForSeconds(delay);
        newSlot = Mathf.Clamp(newSlot, 0, inventoryManager.inventorySlots.Length - 1);
        inventoryManager.ChangeSelectedSlot(newSlot);
    }
}
