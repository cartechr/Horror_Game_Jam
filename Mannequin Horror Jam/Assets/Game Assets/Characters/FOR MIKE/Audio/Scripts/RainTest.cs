using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class RainTest : MonoBehaviour
{

    GameObject fmodObject;
    FMODEvents fmodEvents;

    void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();

        fmodEvents.startRain(gameObject);

    }

}
