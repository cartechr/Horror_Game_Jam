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
    GameObject Commentary;

    bool hasPlayed = false;

    public GameObject Door;

    private void Awake()
    {
        fmodObject = GameObject.FindGameObjectWithTag("FMODEvents");
        fmodEvents = fmodObject.GetComponent<FMODEvents>();
        animator = GetComponent<Animator>();

        Commentary = GameObject.FindGameObjectWithTag("Commentary");

        fmodEvents.InThere(Door);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.2f);
        Commentary.GetComponent<Commentary>().StartDialogue(Commentary.GetComponent<Commentary>().Dialogue1);
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
