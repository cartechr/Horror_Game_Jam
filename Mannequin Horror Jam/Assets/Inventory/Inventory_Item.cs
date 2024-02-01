using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using static UnityEditor.Progress;

public class Inventory_Item : MonoBehaviour
{
    public Items item;

    [Header("UI")]
    public Image image;

    public void InitialiseItem(Items newItem)
    {
        if (newItem != null)
        {
            if (image != null)
            {
                item = newItem;
                image.sprite = newItem.image;
                Debug.Log("Item initialized successfully.");
            }
        }
    }
}
