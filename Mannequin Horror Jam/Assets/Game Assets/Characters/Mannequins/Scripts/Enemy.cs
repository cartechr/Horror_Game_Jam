using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Tooltip ("Num for State Switch")]
    [SerializeField] int state;


    [Header("Assignments")]
    //public NavMeshAgent navMeshAgent;
    [SerializeField] GameObject player;
    //[SerializeField] GameObject enemyAI;
    //public Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;
    //public Animator enemyAnimator;
    public FPSCONTROL playerInputs;
    public FieldOfView fov; //Use this for to trigger if the player is visible

    [Header("Patrol Waypoints")]
    [Tooltip("Assign Patrol Waypoints Here")]
    [SerializeField] Transform[] patrolWaypoints;
    [Tooltip("Shows the index number of current waypoints")]
    [SerializeField] int currentWaypointIndex;
    [Tooltip("The waiting period once a waypoint is reached")]
    [SerializeField] float waitBetweenWaypoints;
    [Tooltip("Duration for search function when alerted")]
    [SerializeField] float searchDuration;

    [Header("Distances for AI")]
    [SerializeField] float attackDistance;
    [SerializeField] bool playerInAttackRange;

    /* CHANGING THE DISTANCE SETTINGS AND SETTING IT INSIDE FIELD OF VIEW SCRIPT
    [SerializeField] float greenAreaDistance;
    [SerializeField] float yellowAreaDistance;
    [SerializeField] float redAreaDistance;

    */

    [Header("ENEMY AI SETTINGS")]

    [Tooltip("If true the AI will patrol the waypoints")]
    [SerializeField] bool setToPatrol;
    [Tooltip("If true the AI will be harder")]
    [SerializeField] bool maskedMannequin;

    [Header("ENEMY AI STATES")]
    [SerializeField] bool isSearching;
    [SerializeField] bool isPatrolling;
    [SerializeField] bool isDisturbed;

    Vector3 lastKnownPlayerPosition;
    [SerializeField] float PositionSize;

    /* DISTANCE AND FOV WILL BE DETERMINED IN THE FIELD OF VIEW SCRIPT
    [SerializeField] bool playerInGreenArea;
    [SerializeField] bool playerInYellowArea;
    [SerializeField] bool playerInRedArea;

    */

    //[SerializeField] bool isWaiting;
    //[SerializeField] bool isAlerted;

    //[SerializeField] bool isChasing;
    //[SerializeField] bool isAttacking;


    //[SerializeField] Vector2 playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        state = 1;
        player = GameObject.FindWithTag("Player");

        fov = GetComponent<FieldOfView>();
    }

    //Patrolling
    //Chasing
    //Attacking
    //LastKnownLocation

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                Alerted();
                break;
            case 1:
                Patrol();
                break;
            case 2:
                Chase(); 
                break;
            case 3:
                Attack();
                break;
        }

        /*
        playerInGreenArea = Physics.CheckSphere(transform.position, AdjustedDetectionRadius(transform.position, greenAreaDistance), whatIsPlayer);
        playerInYellowArea = Physics.CheckSphere(transform.position, AdjustedDetectionRadius(transform.position, yellowAreaDistance), whatIsPlayer);
        playerInRedArea = Physics.CheckSphere(transform.position, AdjustedDetectionRadius(transform.position, redAreaDistance), whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, AdjustedDetectionRadius(transform.position, attackDistance), whatIsPlayer);
        */

        /* MIGHT NEED TO REVERT BACK
        playerInGreenArea = Physics.CheckSphere(transform.position, greenAreaDistance, whatIsPlayer);
        playerInYellowArea = Physics.CheckSphere(transform.position, yellowAreaDistance, whatIsPlayer);
        playerInRedArea = Physics.CheckSphere(transform.position, redAreaDistance, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer);
        */
    }

    private void Alerted()
    {
        if (Vector3.Distance(lastKnownPlayerPosition, gameObject.transform.position) < PositionSize)
        {
            Debug.Log("Reach last known position");
            this.GetComponent<Animator>().SetBool("isMoving", false);
            if (!isSearching)
            {
                StartCoroutine(IsSearching());
                Debug.Log("Alerted Searching for player");
            }
        }
        else
        {
            Debug.Log("Alerted by player");
            this.GetComponent<NavMeshAgent>().SetDestination(lastKnownPlayerPosition);
            this.GetComponent<Animator>().SetBool("isMoving", true);
            
        }
    }


    private void Patrol()
    {
        if (setToPatrol)
        {

            if (patrolWaypoints.Length == 0)
            {
                Debug.LogError("No waypoints assigned. Please assign waypoints in the inspector");
                return;
            }

            //Stop patrolling and searching for player if currently doing so.
            //A safe guard
            if (isDisturbed)
            {
                Debug.Log("Don't patrol if attemped");
                return;
            }
            if (!isSearching)
            {
                StartCoroutine(IsSearching());
            }

            if (!isPatrolling)
            {
                Debug.Log("Patrolling");
                isPatrolling = true;

                this.GetComponent<NavMeshAgent>().SetDestination(patrolWaypoints[currentWaypointIndex].position);
                this.GetComponent<Animator>().SetBool("isMoving", true);
            }
            if (isPatrolling)
            {
                if (Vector3.Distance(transform.position, patrolWaypoints[currentWaypointIndex].position) < 1f)
                {
                    StartCoroutine(WaitAtWaypoint());
                    currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
                }
            }

        }

        if (!setToPatrol)
        {
            StartCoroutine(IsSearching());
        }

       
    }

    IEnumerator WaitAtWaypoint()
    {
        //Debug.Log("Currently Waiting at Waypoint");
        this.GetComponent<Animator>().SetBool("isMoving", false);

        yield return new WaitForSeconds(waitBetweenWaypoints);
        
        IsSearching();

        Debug.Log("Current waypoint is " + currentWaypointIndex.ToString());
        isPatrolling = false;

    }

    IEnumerator IsSearching()
    {
        isSearching = true;
        Debug.Log("Searching for player");
        if (fov.canSeePlayer && !playerInputs.isCrouching)
        {
            Debug.Log("Player is detected!");
            //Chasing player to Attack (player moved)
            
            if (playerInputs.move != Vector2.zero)
            {
                isSearching = false;
                isDisturbed = true;
                state = 2;
                yield break;
            }
            
        }

        else if (!fov.canSeePlayer)
        {

            Debug.Log("Enemy lost sight");

            if (fov.playerInsideRadius && playerInputs.isSprinting)
            {
                isSearching = false;
                isDisturbed = true;
                lastKnownPlayerPosition = player.transform.position;
                state = 0;
                yield break;
            }


        }

        yield return new WaitForSeconds(searchDuration);
        Debug.Log("Player not found");
        isSearching = false;
        isDisturbed = false;
        state = 1;

        /*
        else if (playerInYellowArea)
        {
            Debug.Log("Player in yellow");
            //Alerted (player walking)
            
            if (playerInputs.move != Vector2.zero && !playerInputs.isSprinting)
            {
                isSearching = false;
                isDisturbed = true;
                lastKnownPlayerPosition = player.transform.position;
                state = 0;
                yield break;
            }

            //Chasing player to Attack (player sprinting)
            if (playerInputs.move != Vector2.zero && playerInputs.isSprinting)
            {
                isSearching = false;
                isDisturbed = true;
                state = 2;
                yield break;
            }

            //Player is crouching in yellow area, continue patrolling
            if (playerInputs.move != Vector2.zero && playerInputs.isCrouching)
            {

                isSearching = false;
                isDisturbed = false;
                state = 1;
                yield break;

            }
            
        }
        */





    }
        

    private void Chase()
    {
        if (!fov.canSeePlayer)
        {
            state = 0;
        }
        else
        {
            Debug.Log("AI is chasing player");
            this.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
            this.GetComponent<Animator>().SetBool("isMoving", true);
            lastKnownPlayerPosition = player.transform.position;

            if (Vector3.Distance(transform.position, player.transform.position) < PositionSize)
            {
                Attack();
            }
        }

    }

    private void Attack()
    {
        Debug.Log("Attack player");

        this.GetComponent<Animator>().SetBool("isMoving", false);


        //PLACEHOLDER
        //-----------
        //-----------
        //-----------
    }

    /*
    private float AdjustedDetectionRadius(Vector3 center, float originalRadius)
    {
        float adjustedRadius = originalRadius;

        // Raycast in multiple directions to find collisions
        Vector3[] directions = { transform.forward, -transform.forward, transform.right, -transform.right, transform.up, -transform.up };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;

            if (Physics.Raycast(center, direction, out hit, originalRadius, whatIsWall))
            {
                Debug.DrawRay(center, direction * hit.distance, Color.blue); // Add this line after the Physics.Raycast line
                Debug.Log($"Ray hit at distance: {hit.distance}");


                // Adjust the radius based on the distance to the hit point
                adjustedRadius = Mathf.Min(adjustedRadius, hit.distance);
            }
        }

        return adjustedRadius;
    }
    */

    /*
    private void OnDrawGizmos()
    {

        DrawAdjustedDetectionRange(transform.position, greenAreaDistance, Color.green);
        DrawAdjustedDetectionRange(transform.position, yellowAreaDistance, Color.yellow);
        DrawAdjustedDetectionRange(transform.position, redAreaDistance, Color.red);
        DrawAdjustedDetectionRange(transform.position, attackDistance, Color.magenta);

        
        // Draw Gizmos for the detection ranges
        DrawDetectionRange(transform.position, greenAreaDistance, Color.green);
        DrawDetectionRange(transform.position, yellowAreaDistance, Color.yellow);
        DrawDetectionRange(transform.position, redAreaDistance, Color.red);
        DrawDetectionRange(transform.position, attackDistance, Color.magenta);
        
    }

    private void DrawAdjustedDetectionRange(Vector3 center, float originalRadius, Color color)
    {
        Gizmos.color = color;

        float adjustedRadius = AdjustedDetectionRadius(center, originalRadius);

        Gizmos.DrawWireSphere(center, adjustedRadius);
    }
    */

    /*
    private void DrawDetectionRange(Vector3 center, float radius, Color color)
    {

        Gizmos.color = color;
        Gizmos.DrawWireSphere(center, radius);
        
    }
    */

}
