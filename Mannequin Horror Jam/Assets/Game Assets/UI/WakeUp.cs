using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    public GameObject fmodObject;
    FMODEvents fmodEvents;
    public GameObject AnimatedSarah;
    Animator animator;
    public GameObject Commentary;

    bool hasPlayed = false;

    public GameObject Door;


    // Start is called before the first frame update
    void Start()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();
        animator = GetComponent<Animator>();

        fmodEvents.InThere(Door);

        Commentary.GetComponent<Commentary>().WakeUp();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WakeUP());
    }

    IEnumerator WakeUP()
    {
        yield return new WaitForSeconds(3);

        if (!hasPlayed)
        {
            animator.SetBool("WakingUp", true);
            fmodEvents.startSarah();
        }
        hasPlayed = true;
    }
    private void IsAwake()
    {
        gameObject.SetActive(false);
        AnimatedSarah.SetActive(true);

    }
}
