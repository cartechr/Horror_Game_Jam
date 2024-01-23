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
    public bool isChasing;
    public bool isAttacking;

    [SerializeField] Vector2 playerMovement;

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

        if(!playerInGreenArea && !playerInYellowArea && 
            !playerInRedArea && !playerInAttackRange &&!isWaiting
            && !isSearching && !isAttacking && !isChasing)
        {
            Patrolling();
        }

        if (playerInGreenArea)
        {

            //Check if the player is sprinting
            if(playerInputs.sprint == true && playerMovement != Vector2.zero)
            {
                //Player is sprinting, go to Alert state
                Alerted();
            }
            
        }

        if (playerInYellowArea)
        {

            if (playerInputs.crouching)
            {
                Alerted();
            }

            if (!playerInputs.crouching)
            {
                Chase();
            }

        }

    }

    void Alerted()
    {
        Debug.Log("AI is Alerted");
        Searching();
        //StartCoroutine(WaitAtWaypoint());

    }

    void Searching()
    {

        isSearching = true;
        isChasing = false;
        Debug.Log("isSearching: " + isSearching);
        StartCoroutine(AlertedDuration());

        Vector3 lastKnownPlayerPosition = playerTransform.position;
        enemyAnimator.SetBool("isMoving", true);
        navMeshAgent.SetDestination(lastKnownPlayerPosition);

        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
        {
            Debug.Log("Reached Search Destination");
            StartCoroutine(WaitAtWaypoint());
        }

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
        isSearching = true;
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
        isSearching = false;

    }

    IEnumerator AlertedDuration()
    {

        float elapsedTime = 0f;
        while (elapsedTime < searchDuration)
        {
            isAlerted = true;
            Debug.Log("isAlerted" + isAlerted);

            elapsedTime += Time.deltaTime;

            yield return null;

            isAlerted = false;
            Debug.Log("isAlerted" + isAlerted);
        }

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
