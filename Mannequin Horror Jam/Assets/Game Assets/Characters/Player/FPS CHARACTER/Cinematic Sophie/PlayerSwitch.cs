using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    public GameObject player;

    public GameObject fmodObject;
    FMODEvents fmodEvents;

    GameObject Door; 

    private void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();

        Door = GameObject.FindGameObjectWithTag("Door");

        fmodEvents.startSarah();
        //fmodEvents.InThere(Door);

    }
    void SwitchToPlayer()
    {
        player.SetActive(true);
        this.gameObject.SetActive(false);


    }


}
