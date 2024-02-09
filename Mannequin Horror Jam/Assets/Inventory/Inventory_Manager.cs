using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class Inventory_Manager : MonoBehaviour
{
    public static Inventory_Manager Instance;

    public InventorySlot[] inventorySlots;
    public NotesSlot[] noteSlots;
    public int ActiveNoteNum;

    public GameObject inventoryItemPrefab;

    public int selectedSlot = -1;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    public void ChangeSelectedSlot(int newValue)
    {
        // Ensure circular behavior when reaching the first or last slot 
        if (newValue < 0)
        {
            newValue = inventorySlots.Length - 1;
        }
        else if (newValue >= inventorySlots.Length)
        {
            newValue = 0;
        }


        // Deselect the currently selected slot 
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        // Update the selected slot 
        selectedSlot = newValue;

        // Select the new slot 
        inventorySlots[selectedSlot].Select();
    }

    public Items GetSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
            InventorySlot slot = inventorySlots[selectedSlot];

            // Log to check if the slot is valid 
            if (slot != null)
            {
                Inventory_Item itemInSlot = slot.gameObject.GetComponent<Inventory_Item>();

                // Log to check if the Inventory_Item component is found 
                if (itemInSlot != null)
                {
                    return itemInSlot.item;
                }
                else
                {
                    Debug.LogWarning("Inventory_Item component is null for selected slot.");
                }
            }
            else
            {
                Debug.LogWarning("Selected slot GameObject is null.");
            }
        }
        else
        {
            Debug.LogWarning("Selected slot index is out of range.");
        }

        return null;
    }
    public bool AddItem(Items item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Items item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        Inventory_Item inventoryItem = newItemGo.GetComponent<Inventory_Item>();
        inventoryItem.InitialiseItem(item);
    }




    //--------------------------------------------------------------------------
    //NOTES

    /* public bool AddNote(Notes note)
    {
        for (int i = 0; i < noteSlots.Length; i++)
        {
            NotesSlot slot = noteSlots[i];

        }
    }*/

}
