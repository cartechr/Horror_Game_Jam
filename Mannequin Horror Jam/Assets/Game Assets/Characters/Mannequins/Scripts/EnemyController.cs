using System.Collections;
using System.Collections.Generic;
using StarterAssets;
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

    [Space(10)]
    [Header("Patrol Variables")]
    [Tooltip("Time in which AI is in idle state")]
    [SerializeField] float idleTime;
    [Tooltip("Duration for search function when alerted")]
    [SerializeField] float searchDuration;

    [Space(10)]
    [Tooltip("Current Assigned Waypoint")]
    [SerializeField] int currentWaypoint;
    [Tooltip("Assign Patrol Waypoints Here")]
    [SerializeField] Transform[] patrolWaypoints;

    [Space(10)]
    [Header("Player Detection")]
    [Tooltip("Reference to Player")]
    public GameObject playerRef;
    [Tooltip("Can AI see player?")]
    public bool canSeePlayer;
    [Tooltip("Player Layer Mask")]
    public LayerMask targetMask;
    [Tooltip("Wall Layer Mask")]
    public LayerMask wallMask;
    public float radius;

    private void Start()
    {
        state = 1;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SearchRoutine());
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, wallMask))
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer= false;
        }
        
    }

   private void Patrol()
    {
        if (patrolWaypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned. Please assign waypoints in the inspector");
            return;
        }

        Debug.Log("Patrolling");
        this.GetComponent<NavMeshAgent>().SetDestination(patrolWaypoints[currentWaypoint].position);
        this.GetComponent<Animator>().SetBool("isMoving", true);

        if (Vector3.Distance(transform.position, patrolWaypoints[currentWaypoint].position) < 1.2f)
        {
            currentWaypoint = (currentWaypoint + 1) % patrolWaypoints.Length;
            Debug.Log("Current Waypoint is " + currentWaypoint);
            this.GetComponent<Animator>().SetBool("isMoving", false);

            state = 2;
        }

    }

    private void Idle()
    {
        if (state != 2)
        {
            return;
        }

        StartCoroutine(WaitAtWayPoint());
    }
    IEnumerator WaitAtWayPoint()
    {
        if (state == 2)
        {
            Debug.Log("Waiting " + idleTime + " seconds");
            yield return new WaitForSeconds(idleTime);
            state = 1;
        }
        else
        {
            yield break;
        }
    }

    private void Alerted()
    {
        if (state != 3)
        {
            return;
        }
    }

    private void Chase()
    {
        if (state != 4)
        {
            return;
        }
    }

    private void Attack()
    {
        if (state != 5)
        {
            return;
        }
    }

    private void Stunned()
    {
        if (state != 6)
        {
            return;
        }
    }
}
