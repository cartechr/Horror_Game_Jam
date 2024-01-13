using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_Inventory : MonoBehaviour
{
    [Header("Current Item")]
    public string Active;

    [Header("Slot 1")]
    public string Slot1_gameObject;
    public bool Slot1_Active = false;

    [Header("Slot 2")]
    public string Slot2_gameObject;
    public bool Slot2_Active = false;

    public GameObject pipe_Hand;
    public GameObject flashlight_Hand;

    public GameObject one_flashlight;
    public GameObject one_Pipe;

    public GameObject two_flashlight;
    public GameObject two_Pipe;

    // Update is called once per frame
    void Update()
    {
        
    }
}
