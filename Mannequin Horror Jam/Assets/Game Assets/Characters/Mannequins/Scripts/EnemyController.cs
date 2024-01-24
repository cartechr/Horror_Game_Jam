using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{

    [Header("Assignments")]
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public GameObject enemyAI;
    public Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator enemyAnimator;
    public StarterAssets.PlayerInputs playerInputs;

    [Header("Patrol Waypoints")]
    [Tooltip("Assign Patrol Waypoints Here")]
    public Transform[] patrolWaypoints;
    [Tooltip("Shows the index number of current waypoint")]
    public int currentWaypointIndex;
    [Tooltip("The waiting period once a waypoint is reached")]
    public float waitBetweenWaypoints;
    [Tooltip("Duration for search function when alerted")]
    public float searchDuration;

    [Header("Distances for AI")]
    public float greenAreaDistance;
    public float yellowAreaDistance;
    public float redAreaDistance;
    public float attackDistance;

    [Header("States for Player & Distance")]
    public bool playerInGreenArea;
    public bool playerInYellowArea;
    public bool playerInRedArea;
    public bool playerInAttackRange;
    public bool isWaiting;
    public bool isAlerted;
    public bool isSearching;
    public bool isPatrolling;
    public bool isChasing;
    public bool isAttacking;

    [SerializeField] Vector2 playerMovement;

    Vector3 lastKnownPlayerPosition;
    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        currentWaypointIndex = 0;

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        playerMovement = playerInputs.move;

        playerInGreenArea = Physics.CheckSphere(transform.position, greenAreaDistance, whatIsPlayer);
        playerInYellowArea = Physics.CheckSphere(transform.position, yellowAreaDistance, whatIsPlayer);
        playerInRedArea = Physics.CheckSphere(transform.position, redAreaDistance, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer);

        /*
        if(!playerInGreenArea && !playerInYellowArea && 
            !playerInRedArea && !playerInAttackRange &&!isWaiting
            && !isSearching && !isAttacking && !isChasing)
        {
            Patrolling();
        }
        */

        if (isPatrolling)
        {
            Patrolling();
        }

        if (playerInGreenArea) //Player enters the green area
        {
            Debug.Log("Player is in Green Area");

            //Check if the player is sprinting
            if (playerInputs.sprint == true && playerMovement != Vector2.zero)
            {

                Debug.Log("Sprint is detected in Green Area");
                isPatrolling = false;
                Debug.Log("AI is no longer patrolling");
                Debug.Log("isPatrolling " + isPatrolling);

                lastKnownPlayerPosition = playerTransform.position;
                Debug.Log("Player Last Known location memorized ");

                isAlerted = true;
                Debug.Log("isAlerted " + isAlerted);

            }

        }

        if(isAlerted)
        {

            enemyAnimator.SetBool("isMoving", true);
            navMeshAgent.SetDestination(lastKnownPlayerPosition);
            Debug.Log("Enemy is moving towards player last known location");

            // Check if the agent has reached its destination
            if (!navMeshAgent.pathPending && !navMeshAgent.hasPath && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Agent has reached the destination
                Debug.Log("Enemy reached destination");
                enemyAnimator.SetBool("isMoving", false);
                //StartCoroutine(IsSearching());
            }

        }



        if (playerInYellowArea)
        {

            if (!playerInputs.crouching)
            {
                Chase();
            }

        }

    }


    IEnumerator IsSearching()
    {
        Debug.Log("The AI reached to the destination and searching for player ");
        isSearching = true;
        Debug.Log("isSearching " + isSearching);

        float elapsedTime = 0f;
        while (elapsedTime < searchDuration)
        {

            elapsedTime += Time.deltaTime;

            Debug.Log("AI is searching at the location...");
            Debug.Log("Time Elapsed: " + elapsedTime);



            yield return null;


        }

    }

    void CheckDistance()
    {
        Vector3 lastKnownPlayerPosition = playerTransform.position;
        
    }

    void Alerted()
    {

        
        /*
        //Check if the player is sprinting
        if (playerInputs.sprint == true && playerMovement != Vector2.zero)
        {

            //If player is sprinting go to alerted stage
            Debug.Log("AI is Alerted");
            isAlerted = true;
            Debug.Log("isAlerted: " + isAlerted);

            isSearching = true;
            Debug.Log("isSearching: " + isSearching);

            isChasing = false;
            Debug.Log("isChasing: " + isChasing);


            enemyAnimator.SetBool("isMoving", true);
            navMeshAgent.SetDestination(lastKnownPlayerPosition);
            Debug.Log("Enemy Moving Towards Player Location");

        }

        if (distanceToPlayer < 1f)
        {
            Debug.Log("Reached Search Destination");
        }

        Searching();
        */

    }

    void Searching()
    {

        

    }



    void Chase()
    {
        enemyAnimator.SetBool("isMoving", true);
        Debug.Log("AI is Chasing");
        Debug.Log("Stopping Searching");
        isSearching = false;
        isChasing = true;
        Vector3 lastKnownPlayerPosition = playerTransform.position;
        navMeshAgent.SetDestination(lastKnownPlayerPosition);

    }

    void Attack()
    {
        Debug.Log("AI is Attacking");
    }

    void Patrolling()
    {
        Debug.Log("AI is Patrolling");
        // Check if there are waypoints
        if (patrolWaypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned. Please assign waypoints in the inspector.");
            return;
        }

        // Set destination to the current waypoint
        navMeshAgent.SetDestination(patrolWaypoints[currentWaypointIndex].position);
        enemyAnimator.SetBool("isMoving", true);

        // Check if the enemy has reached the current waypoint
        if (Vector3.Distance(transform.position, patrolWaypoints[currentWaypointIndex].position) < 1f)
        {
            //Wait at waypoint
            StartCoroutine(WaitAtWaypoint());
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
        }

    }

    IEnumerator WaitAtWaypoint()
    {
        isPatrolling = false;
        Debug.Log("isPatrolling: " + isPatrolling); 
        Debug.Log("Waiting at waypoint...");

        //Wait for the specified time
        float elapsedTime = 0f;
        while (elapsedTime < waitBetweenWaypoints)
        {
            enemyAnimator.SetBool("isMoving", false);
            elapsedTime += Time.deltaTime;
            
            yield return null;

        }

        Debug.Log("Finished waiting at waypoint");
        isPatrolling = true;
        Debug.Log("isPatrolling: " + isPatrolling);

    }



    private void OnDrawGizmos()
    {
        // Draw Gizmos for the detection ranges
        DrawDetectionRange(transform.position, greenAreaDistance, Color.green);
        DrawDetectionRange(transform.position, yellowAreaDistance, Color.yellow);
        DrawDetectionRange(transform.position, redAreaDistance, Color.red);
        DrawDetectionRange(transform.position, attackDistance, Color.magenta);
    }
    private void DrawDetectionRange(Vector3 center, float radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(center, radius);
    }

}
