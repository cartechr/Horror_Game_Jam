using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public GameObject fmodObject;
    FMODEvents fmodEvents;
    bool hasPlayed = false;

    public GameObject Door;
    public GameObject Note;

    GameObject Commentary;

    // Start is called before the first frame update
    void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();

        Commentary = GameObject.FindGameObjectWithTag("Commentary");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasPlayed)
            {
                hasPlayed = true;
                fmodEvents.finAwake(Door);
                Commentary.GetComponent<Commentary>().StartDialogue(Commentary.GetComponent<Commentary>().Dialogue2);
                Commentary.GetComponent<Commentary>().subtitleGO.SetActive(true);
                
                Note.GetComponent<Animator>().SetBool("Note", true);
            }
        }
    }
}
