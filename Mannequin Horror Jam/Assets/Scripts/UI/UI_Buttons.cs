using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class UI_Buttons : MonoBehaviour
{
    public UI_Buttons[] Notes;

    int num = 1;
    public GameObject PageNum;
    public GameObject ForwardArrow;
    public GameObject BackArrow;

    public TextMeshProUGUI text;
    
    public void ForwardPage()
    {
        if (num < 3)
        {
            num++;
            Debug.Log("Page " + num);

            text.SetText("Pg " + num);
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
            Debug.Log("Page " + num);
            text.SetText("Pg " + num);
        }
        else
        {
            return;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        switch (num)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
