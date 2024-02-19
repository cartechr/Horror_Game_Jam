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

    private void Awake()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();
        animator = GetComponent<Animator>();

        fmodEvents.InThere(Door);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
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
