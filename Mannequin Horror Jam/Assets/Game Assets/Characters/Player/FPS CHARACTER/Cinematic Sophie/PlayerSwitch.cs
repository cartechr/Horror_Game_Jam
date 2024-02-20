using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    public GameObject player;
    public GameObject cursorUI;

    public GameObject fmodObject;
    FMODEvents fmodEvents;


    private void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();

        //fmodEvents.startSarah();

    }
    void SwitchToPlayer()
    {
        player.SetActive(true);
        this.gameObject.SetActive(false);
        cursorUI.SetActive(true);


    }


}
