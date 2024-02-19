using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public GameObject fmodObject;
    FMODEvents fmodEvents;
    bool hasPlayed = false;

    public GameObject Door;

    // Start is called before the first frame update
    void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasPlayed)
            {
                hasPlayed = true;
                fmodEvents.finAwake(Door);
            }
        }
    }
}
