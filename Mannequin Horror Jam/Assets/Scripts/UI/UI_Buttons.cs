using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class UI_Buttons : MonoBehaviour
{

    public int num;
    Inventory_Manager inventoryManager;



    [Space(10)]
    [Header("Buttons")]
    public GameObject ForwardArrow;
    public GameObject BackArrow;

    [Space(10)]
    [Header("TextMeshPro")]
    public TextMeshProUGUI Settings_UI;
    public TextMeshProUGUI Items_UI;
    public TextMeshProUGUI Notes_UI;

    [Space(10)]
    [Header("Inv Slots")]
    //public GameObject settings;
    public GameObject items;
    public GameObject notes;

    Color notActive = new Color32(255, 255, 255, 255);
    Color Active = new Color32(255, 184, 20, 255);

    public void ForwardPage()
    {
        if (num < 3)
        {
            num++;
            Debug.Log(num);
        }
        else
        {
            return;
        }

    }
   public void BackPage() 
    {
        if (num > 1)
        {
            num--;
            Debug.Log(num);
        }
        else
        {
            return;
        }
    }

    private void Start()
    {
        inventoryManager = GetComponent<Inventory_Manager>();
    }

    private void Update()
    {
        switch (num)
        {
            //Settings
            case 1:
                Settings();
                break;
            //Items
            case 2:
                Item();
                break;
            //Notes
            case 3:
                Note();
                break;
        }
    }

    private void Settings()
    {
        if (Settings_UI.color != Active)
        {
            //UI color
            Settings_UI.color = Active;
            Items_UI.color = notActive;
            Notes_UI.color = notActive;

            //UI Slots
            //settings.SetActive(true);
            items.SetActive(false);
            notes.SetActive(false);
        }
    }

    private void Item()
    {
        if (Items_UI.color != Active)
        {
            //UI color
            Settings_UI.color = notActive;
            Items_UI.color = Active;
            Notes_UI.color = notActive;

            //UI Slots
            //settings.SetActive(false);
            items.SetActive(true);
            notes.SetActive(false);
        }
    }

    private void Note()
    {
        if (Notes_UI.color != Active)
        {
            //UI color
            Settings_UI.color = notActive;
            Items_UI.color = notActive;
            Notes_UI.color = Active;

            //UI Slots
            //settings.SetActive(false);
            items.SetActive(false);
            notes.SetActive(true);
        }
    }
}
