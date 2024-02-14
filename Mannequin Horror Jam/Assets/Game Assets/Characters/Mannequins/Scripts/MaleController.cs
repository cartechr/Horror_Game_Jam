using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using StarterAssets;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class MaleController : MonoBehaviour
{
    [Header("Current State")]
    [Space(6)]
    [Tooltip("Current State Number")]
    [SerializeField] int state;
    [Tooltip("Current State Name")]
    [SerializeField] string Name;
    [Tooltip("A bool that checks if a state switch has occured")]
    [SerializeField] bool switchState;
    bool ignoreseekspeed;

    [Space(15)]
    [Header("AI Variables")]

    [Space(6)]
    [Header("Idle")]
    [Tooltip("Time in which AI is in idle state")]
    [SerializeField] float idleTime;
    [Tooltip("How close should AI get to point before idling?")]
    [SerializeField] float WhenAtIdlePoint;
    bool reachedPoint;

    [Space(6)]
    [Header("Grapple")]
    [Tooltip("How long before AI hurts player")]
    [SerializeField] float healthDelay;
    [SerializeField] float timeStart;
    [Tooltip("How long Ai should remain stunned")]
    [SerializeField] float spamDecider = 4;
    [SerializeField] float spam;
    [SerializeField] float spamDamage;
    [SerializeField] float aiSpamDamage;

    [Space(6)]
    [Header("Stun")]
    [SerializeField] float stunTime;

    [Space(6)]
    //[SerializeField] int currentWaypoint;
    [Tooltip("Assign Patrol Waypoints Here")]
    [SerializeField] Transform[] patrolWaypoints;

    [Space(10)]
    [Header("Player Detection")]
    [Space(6)]
    [Tooltip("Reference to Player (DONT ADD ANYTHING HERE)")]
    public GameObject playerRef;
    [Tooltip("Player Layer Mask")]
    public LayerMask targetMask;
    [Tooltip("Wall Layer Mask")]
    public LayerMask wallMask;
    public GameObject playerHead;
    [SerializeField] GameObject aiHead;


    [Space(15)]
    [Header("Radius")]
    [Space(6)]
    public float radiusRed;
    public bool inRed;
    public float radiusYellow;
    public bool inYellow;
    public float radiusGreen;
    public bool inGreen;

    public float radiusAttack;
    public bool inAttack;

    Transform lastknownlocation;
    public GameObject Head;




    [Space(10)]
    [Space(6)]
    [Header("FPS Controller")]
    [Tooltip("DONT ADD ANYTHING HERE")]
    public FPSCONTROL fpscontroller;


    NavMeshAgent agent;
    Animator animator;
    int animNum;

    [Space(20)]
    //Button Smashing UI
    float Clock = .2f;
    float startClock = 0;
    public GameObject UI1;
    public GameObject UI2;
    public GameObject barObject;
    bool switchUI = true;


   /* [field: SerializeField] public EventReference mannequinMovement { get; private set; }
    private EventInstance mannequinMovementInst;

    private static PARAMETER_ID GetParamID(EventInstance instance, ParamRef parameter)
    {
        instance.getDescription(out EventDescription eventDesc);
        eventDesc.getParameterDescriptionByName(parameter.Name, out PARAMETER_DESCRIPTION paramDesc);
        return paramDesc.id;
    }*/


    private void Start()
    {
        state = 1;
        switchState = true;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        fpscontroller = playerRef.GetComponent<FPSCONTROL>();
        StartCoroutine(SearchRoutine());
        //animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Head = GameObject.FindGameObjectWithTag("Head");
        playerHead = GameObject.FindGameObjectWithTag("CinemachineTarget");
        aiHead = GameObject.FindGameObjectWithTag("aiHead");
        //Bar = GetComponent<Slider>();



        spam = spamDecider;

        animNum = Random.Range(0, 2);
        animator.SetFloat("Idle", animNum);
    }
    private void Update()
    {
        switch (state)
        {
            case 1:
                Stay();
                Name = "Stay";
                break;
            case 2:
                Idle();
                Name = "Idle";
                break;
            case 3:
                Alerted();
                Name = "Alerted";
                break;
            case 4:
                Chase();
                Name = "Chase";
                break;
            case 5:
                Attack();
                Name = "Attack";
                break;
            case 6:
                grappleAttack();
                Name = "grappleAttack";
                break;
            case 7:
                Stunned();
                Name = "Stunned";
                break;
            case 8:
                UnStunned();
                Name = "UnStunned";
                break;
        }
    }

    private IEnumerator SearchRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            SearchCheck();
        }
    }

    private void SearchCheck()
    {
        Debug.Log("Actively Searching");
        Transform target;
        Vector3 directionToTarget;
        float distanceToTarget;

        Collider[] greenChecks = Physics.OverlapSphere(transform.position, radiusGreen, targetMask);
        Collider[] yellowChecks = Physics.OverlapSphere(transform.position, radiusYellow, targetMask);
        Collider[] redChecks = Physics.OverlapSphere(transform.position, radiusRed, targetMask);
        Collider[] attackChecks = Physics.OverlapSphere(transform.position, radiusAttack, targetMask);



        //Green Area
        if (greenChecks.Length != 0 && yellowChecks.Length == 0 && redChecks.Length == 0 && attackChecks.Length == 0)
        {
            target = greenChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, wallMask))
            {
                inGreen = true;
                SeePlayer();
            }
            else
            {
                inGreen = false;
            }
        }

       else if (inGreen)
       {
            inGreen = false;
       }

        //Yellow Area
        if (yellowChecks.Length != 0 && redChecks.Length == 0 && attackChecks.Length == 0)
        {
            target = yellowChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, wallMask))
            {
                inYellow = true;
                SeePlayer();
            }
            else
            {
                inYellow = false;
            }
        }
        else if (inYellow)
        {
            inYellow = false;
            Debug.Log("Not in Yellow");
        }

        //Red Area
        if (redChecks.Length != 0 && attackChecks.Length == 0 && attackChecks.Length == 0)
        {
            target = redChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, wallMask))
            {
                inRed = true;
                SeePlayer();
            }
            else
            {
                inRed = false;
                Debug.Log("Not in red");
            }
        }
        else if (inRed)
        {
            inRed = false;
        }

        if (attackChecks.Length != 0)
        {
            target = attackChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, wallMask))
            {
                inAttack = true;
            }
            else
            {
                inAttack = false;
            }
        }
        else if (inAttack)
        {
            inAttack = false;
        }

    }

    private void SeePlayer()
    {
        if (state == 1 || state == 2 || state == 3)
        {
            //Debug.Log("Can Find Player");
            if (inGreen)
            {
                //Alerted
                if (fpscontroller.isSprinting)
                {
                    lastknownlocation = playerRef.transform;
                    state = 3;

                    reachedPoint = false;
                    switchState = true;
                    //Debug.Log("Alerted");
                    return;
                }
            }

            if (inYellow)
            {
                //Alerted
                if (fpscontroller.isWalking)
                {
                    lastknownlocation = playerRef.transform;
                    state = 3;

                    reachedPoint = false;
                    switchState = true;
                    //Debug.Log("Alerted");
                    return;
                }

                //Chase
                if (fpscontroller.isSprinting && fpscontroller.move != Vector2.zero)
                {
                    state = 4;

                    reachedPoint = false;
                    switchState = true;
                    //Debug.Log("Chasing");
                    return;
                }
            }

            if (inRed)
            {
                //Chase
                if (fpscontroller.isWalking || fpscontroller.isSprinting && fpscontroller.move != Vector2.zero)
                {
                    state = 4;

                    reachedPoint = false;
                    switchState = true;
                    //Debug.Log("Chasing");
                    return;
                }
            }
        }
    }
    private void aiFootSteps()
    {
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.mannequinMovement, this.transform.position);
        // AudioManager.instance.Re

        // mannequinMovementInst = FMODUnity.RuntimeManager.CreateInstance(mannequinMovement);
        // mannequinMovementInst.start();
        //mannequinMovementInst.release();

        // mannequinMovementInst.setParameterByIDWithLabel(mannequinMovement, "PickupObject");


    }

    private void Stay()
    {
        if (patrolWaypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned. Please assign waypoints in the inspector");
            return;
        }

        if (switchState)
        {
            agent.SetDestination(patrolWaypoints[0].position);
            animator.SetBool("isIdle", false);
        }
        if (Vector3.Distance(transform.position, patrolWaypoints[0].position) <= WhenAtIdlePoint)
        {
            if (!reachedPoint)
            {
                reachedPoint = true;
                animator.SetBool("isIdle", true);
                animNum = Random.Range(0, 2);
                animator.SetFloat("Idle", animNum);
            }
        }

        switchState = false;
    }
    private void Idle()
    {
        IEnumerator waitCoroutine = WaitThenReturn();

        //start timer, will transition when it ends
        StartCoroutine(waitCoroutine);

        if (switchState)
        {
            animator.SetBool("isIdle", true);
            animNum = Random.Range(0, 2);
            Debug.Log(animNum);
            animator.SetFloat("Idle", animNum);
        }

        switchState = false;
    }
    IEnumerator WaitThenReturn()
    {
        //float timer = idleTime;
        for (float timeWaited = 0f; timeWaited <= idleTime; timeWaited += Time.deltaTime)
        {
            //Debug.Log(timeWaited);
            if (state != 2)
            {
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (state == 2)
        {
            state = 1;
            switchState = true;
        }
    }

    private void Alerted()
    {
        if (switchState)
        {
            Debug.Log("switched to Alerted State");
            agent.SetDestination(lastknownlocation.position);
            animator.SetBool("isIdle", false);
        }

        if (Vector3.Distance(transform.position, lastknownlocation.position) <= WhenAtIdlePoint)
        {
            animator.SetBool("isIdle", true);

            if (!inAttack)
            {
                state = 2;
                switchState = true;
                return;
            }
            else
            {
                animator.SetBool("isIdle", true);
                agent.SetDestination(transform.position);
                state = 5;
                switchState = true;

                Debug.Log("Attack Player");
                return;
            }
        }
        else
        {
            Debug.Log("Not At Point");
        }
        switchState = false;
    }

    private void Chase()
    {
        if (switchState)
        {
            animator.SetBool("isIdle", false);
        }

        //Head.GetComponent<MultiAimConstraint>().weight = 1.0f;
        agent.SetDestination(playerRef.transform.position);

        if (Vector3.Distance(transform.position, playerRef.transform.position) <= radiusAttack)
        {
            //Head.GetComponent<MultiAimConstraint>().weight = 0f;
            animator.SetBool("isIdle", true);
            agent.SetDestination(transform.position);

            state = 5;
            switchState = true;

            Debug.Log("Attack Player");
            return;
        }

        Debug.Log("Chasing Chasing Chasing");
        if (!inGreen && !inYellow && !inRed && !inAttack)
        {
            Debug.Log("Lost Player");
            Head.GetComponent<MultiAimConstraint>().weight = 0f;
            //agent.SetDestination(lastknownlocation.position);
            state = 3;
            switchState = true;

            return;
        }

        lastknownlocation = playerRef.transform;
        switchState = false;
    }

    private void Attack()
    {
        if (switchState)
        {
            Debug.Log("Attacking Player fdjdglfkg");

            //Face Each Other
            this.transform.LookAt(new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z));
            playerRef.transform.LookAt(new Vector3(transform.position.x, playerRef.transform.position.y, transform.position.z));

            //Player looks at AI
            playerHead.transform.LookAt(aiHead.transform.position);


            fpscontroller.disableLook = true;
            fpscontroller.disableMovement = true;
            animator.SetBool("isAttack", true);
        }

        switchState = false;
    }

    private void switchToGrapple()
    {
        switchState = true;
        state = 6;
        Debug.Log("Play grapple");
    }
    private void grappleAttack()
    {
        if (switchState)
        {
            animator.SetBool("isAttack", false);
            animator.SetBool("IsGrapple", true);

            barObject.gameObject.SetActive(true);
        }



        if (switchUI)
        {
            UI1.gameObject.SetActive(true);
            UI2.gameObject.SetActive(false);
        }
        else
        {
            UI1.gameObject.SetActive(false);
            UI2.gameObject.SetActive(true);
        }

        if (startClock < Clock)
        {
            startClock += Time.deltaTime;
        }
        else
        {
            startClock = 0;

            if (switchUI)
            {
                switchUI = false;
            }
            else
            {
                switchUI = true;
            }
        }


        //if player is currently still alive and grapple hasnt ended
        if (fpscontroller.Health != 0 && spam <= 6)
        {
            //Losing Struggle/Health
            if (spam > 1)
            {
                spam -= aiSpamDamage * Time.deltaTime;
                barObject.GetComponent<Slider>().value = spam;
                animator.SetFloat("Grapple", spam);
            }
            else
            {
                spam = 1;
                animator.SetFloat("Grapple", spam);
            }

            if (timeStart < healthDelay)
            {
                timeStart += Time.deltaTime;
            }
            else
            {
                timeStart = 0;

                //Decrease player health
                fpscontroller.Health -= 1;

                //Reset regen timer
                fpscontroller.timeStart = 0;

                Debug.Log("Player currently has " + fpscontroller.Health + " health");
            }

            //Fighting/Winning Struggle
            if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.A)) && (Input.GetKeyDown(KeyCode.D)))
            {
                spam += spamDamage;
                barObject.GetComponent<Slider>().value = spam;
                animator.SetFloat("Grapple", spam);
            }

        }
        else
        {

            /* if (playerHealth == 0)
             {

                 return;
                 //Player dies
             }*/

            if (spam >= 6)
            {
                startClock = 0;
                state = 7;
                timeStart = 0;

                switchState = true;

                spam = spamDecider;
                UI1.gameObject.SetActive(false);
                UI2.gameObject.SetActive(false);
                barObject.gameObject.SetActive(false);
                barObject.GetComponent<Slider>().value = spam;


                return;
                //switch to Stun
            }
        }



        switchState = false;
    }
    private void Stunned()
    {
        //Debug.Log("Play Stun");
        //state 7
        if (switchState)
        {
            animator.SetBool("IsGrapple", false);
            animator.SetBool("Stunned", true);

            fpscontroller.disableLook = false;
            fpscontroller.disableMovement = false;
        }
        switchState = false;
    }

    private void switchToUnStunned()
    {
        StartCoroutine(StunTime());
    }
    IEnumerator StunTime()
    {
        yield return new WaitForSeconds(stunTime);
        state = 8;
        switchState = true;
        //Debug.Log("Play UnStun");
        
    }

    private void UnStunned()
    {
        //state 8
        if (switchState)
        {
            animator.SetBool("Stunned", false);
            animator.SetBool("UnStunned", true);
        }
        switchState = false;
    }
    private void switchToIdle()
    {
        state = 2;
        animator.SetBool("UnStunned", false);
        switchState = true;
    }
}
