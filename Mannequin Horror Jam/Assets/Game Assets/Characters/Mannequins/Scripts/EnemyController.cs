using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StarterAssets;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public class EnemyController : MonoBehaviour
{
    [Header("Current State")]
    [Tooltip("Current State Number")]
    [SerializeField] int state;
    [Tooltip("Current State Name")]
    [SerializeField] string Name;
    [Tooltip("A bool that checks if a state switch has occured")]
    [SerializeField] bool switchState;

    [Space(10)]
    [Header("AI Variables")]
    [Tooltip("Time in which AI is in idle state")]
    [SerializeField] float idleTime;
    [Tooltip("How close should AI get to point before idling?")]
    [SerializeField] float WhenAtIdlePoint;
    [SerializeField] float Clock;
    [Tooltip("Number player has to reach to win the grapple")]
    [SerializeField] int playerWins;
    [Tooltip("Numer mannequin has to reach to win the grapple")]
    [SerializeField] int mannequinWins;

    [Space(10)]
    [Tooltip("Current Assigned Waypoint")]
    [SerializeField] int currentWaypoint;
    [Tooltip("Assign Patrol Waypoints Here")]
    [SerializeField] Transform[] patrolWaypoints;

    [Space(10)]
    [Header("Player Detection")]
    [Tooltip("Reference to Player (DONT ADD ANYTHING HERE)")]
    public GameObject playerRef;
    [Tooltip("Player Layer Mask")]
    public LayerMask targetMask;
    [Tooltip("Wall Layer Mask")]
    public LayerMask wallMask;


    [Space(15)]
    [Header("Radius")]
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
    [Header("FPS Controller")]
    [Tooltip("DONT ADD ANYTHING HERE")]
    public FPSCONTROL fpscontroller;
    NavMeshAgent agent;
    Animator animator;
    int animNum;


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

        animNum = Random.Range(0, 2);
        animator.SetFloat("Idle", animNum);

        //Debug.Log ("Array " + animNames.Length);
    }
    private void Update()
    {
        switch (state)
        {
            case 1:
                Patrol();
                Name = "Patrol";
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
                    switchState = true;
                    Debug.Log("Alerted");
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
                    switchState = true;
                    Debug.Log("Alerted");
                    return;
                }

                //Chase
                if (fpscontroller.isSprinting && fpscontroller.move != Vector2.zero)
                {
                    state = 4;
                    switchState = true;
                    Debug.Log("Chasing");
                    return;
                }
            }

            if (inRed)
            {
                //Chase
                if (fpscontroller.isWalking || fpscontroller.isSprinting && fpscontroller.move != Vector2.zero)
                {
                    state = 4;
                    switchState = true;
                    Debug.Log("Chasing");
                    return;
                }
            }
        }
    }

   private void Patrol()
   {
        if (patrolWaypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned. Please assign waypoints in the inspector");
            return;
        }

        if (switchState)
        {
            agent.SetDestination(patrolWaypoints[currentWaypoint].position);
            animator.SetBool("isIdle", false);
        }

        if (Vector3.Distance(transform.position, patrolWaypoints[currentWaypoint].position) < WhenAtIdlePoint)
        {
            currentWaypoint = (currentWaypoint + 1) % patrolWaypoints.Length;
            //Debug.Log("Current Waypoint is " + currentWaypoint);
            switchState = true;
            state = 2;
            return;
        }

        switchState = false;
    }

    private void Idle()
    {
        IEnumerator waitCoroutine = WaitThenPatrol();
        
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
    IEnumerator WaitThenPatrol()
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
            agent.SetDestination(lastknownlocation.position);
            animator.SetBool("isIdle", false);
        }

        if (Vector3.Distance(transform.position, lastknownlocation.position) < WhenAtIdlePoint)
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
                Debug.Log("Attack Player");
                state = 5;
                switchState = true;
                return;
            }
        }
        switchState = false;
    }

    private void Chase()
    {
        if (switchState)
        {
            animator.SetBool("isIdle", false);
        }

        Head.GetComponent<MultiAimConstraint>().weight = 1.0f;
        agent.SetDestination(playerRef.transform.position);

        if (Vector3.Distance(transform.position, playerRef.transform.position) <= radiusAttack)
        {
            Head.GetComponent<MultiAimConstraint>().weight = 0f;
            animator.SetBool("isIdle", true);
            agent.SetDestination(transform.position);
            state = 5;
            switchState = true;
            return;
        }

        switchState = false;


      /*  if (inAttack)
        {
            state = 5;

            animator.SetBool("isAttacking", true);

            agent.SetDestination(transform.position);
            playerRef.transform.LookAt(transform.position);
            fpscontroller.disableLook = true;
            fpscontroller.disableMovement = true;

            Debug.Log("Attacking");
            return;
        }
        else
        {
            animator.SetBool("isIdle", false);
            agent.SetDestination(playerRef.transform.position);
            Head.GetComponent<MultiAimConstraint>().weight = 1.0f;
        } */

    }

    private void Attack()
    {
        playerRef.transform.LookAt(transform.position);
        fpscontroller.disableLook = true;
        fpscontroller.disableMovement = true;

    }

    private void grappleAttack()
    {
       /* int Decider = 0;

        for (float startClock = 0f; startClock < Clock; startClock += Time.deltaTime)
        {

            //Player presses one of these buttons in time
            if (Input.GetKeyDown(KeyCode.A) && startClock < Clock || Input.GetKeyDown(KeyCode.D) && startClock < Clock)
            {
                //player wins struggle
                if (Decider == playerWins)
                {
                    state = 6; 
                    break;
                }

                startClock = 0f;
                Decider++;
            }
            //Player doesn't press a button in time
            if (startClock > Clock)
            {
                if (Decider != mannequinWins) 
                {
                    Decider--;
                }

                //player losing struggle
                else
                {
                    //Player take damage

                    //if player takes enough damage
                    //Stop state machine or add a state that kills the player
                    //state = 0; //Not a state machine state, so state machine should "Stop"
                    //break;
                }

                startClock = 0f;
            }
        }*/
    }

    private void Stunned()
    {
        //state 6
    }
}
