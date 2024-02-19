using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Play : MonoBehaviour, IPointerEnterHandler
{
    public GameObject fmodObject;
    public FMODEvents fmodEvents;

    void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();
    }
    public void OnPointer()
    {
        Debug.Log("Pointer over!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        Debug.Log("Play");
        fmodEvents.pressButton();
    }
}
