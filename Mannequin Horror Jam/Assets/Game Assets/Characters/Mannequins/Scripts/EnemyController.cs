using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyController : MonoBehaviour
{

    [Header("Patrol Point Assignments")]
    [Tooltip("Assign Relevant Patrolling Waypoints")]
    [SerializeField] Transform[] patrolWaypoints;

    [Header("Enemy Control Assignments")]
    [Tooltip("Assign Enemy Object")]
    [SerializeField] GameObject mannequinEnemy;
    [Tooltip("Assign Player Here")]
    [SerializeField] GameObject player;
    [Tooltip("Eyes to emit raycast from")]
    [SerializeField] GameObject eyes;
    [Tooltip("Assign the NavMesh Agent (It's inside the mannequin)")]
    [SerializeField] NavMeshAgent navMeshAgent;
    [Tooltip("Assign the Enemy Animator Component")]
    [SerializeField] Animator enemyAnimatorController;
    [Tooltip("This will be pulled automatically")]
    [SerializeField] MultiAimConstraint multiAimConstraint;
    [Tooltip("Select the relevant layers")]
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    [Header("Enemy Control Variables")]
    [Tooltip("Assign the speed for the enemy")]
    [SerializeField] float moveSpeed;
    [Tooltip("Delay between waypoints")]
    [SerializeField] float delayBetween;
    [Tooltip("Float variable for head turn DO NOT CHANGE")]
    [SerializeField] float weight;
    [Tooltip("Head turn weight increase duration")]
    [SerializeField] float increaseDuration = 3f;

    [Header("Adjust Ranges for the AI")]
    [Tooltip("Distance that AI will only be ALERTED if player is sprinting")]
    [SerializeField] float greenRange;
    [Tooltip("Distance that AI only be ALERTED if player is walking and ATTACK if player is sprinting")]
    [SerializeField] float yellowRange;
    [Tooltip("Distance that AI will be ALERTED if the player is walking & crouching but ATTACK if the player is sprinting")]
    [SerializeField] float redRange;
    [Tooltip("Distance for AI attack range")]
    [SerializeField] float attackRange; //This is to decide how far/close should the attack anim happen

    [Header("AI State Controls")]
    [Tooltip("This AI will Patrol & Play Relevant Animations")]
    [SerializeField] bool shouldPatrol;
    [Tooltip("This AI will not patrol but Play Relevant Animations")]
    [SerializeField] bool shouldIdle;
    [Tooltip("Is this AI hostile?")]
    [SerializeField] bool isHostile;

    [Header("DEBUG CONTROLS")]
    [Tooltip("This is for debugging and animation triggering")]
    [SerializeField] bool isMoving;
    [Tooltip("This is for debugging and animation triggering")]
    [SerializeField] bool isChasingPlayer;
    [Tooltip("To check if the player is in green range")]
    [SerializeField] bool playerInGreenRange;
    [Tooltip("To check if the player is in yellow range")]
    [SerializeField] bool playerInYellowRange;
    [Tooltip("To check if the player is in red range")]
    [SerializeField] bool playerInRedRange;
    [Tooltip("To check if the player is in attack range")]
    [SerializeField] bool playerInAttackRange;
    [Tooltip("Check if the player is sprinting")]
    [SerializeField] bool playerIsSprinting;

    private void Awake()
    {
        StarterAssetsInputs starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        playerIsSprinting = starterAssetsInputs.sprint;
    }


    private void Update()
    {
        
        //Checks for player if detected
        playerInGreenRange = Physics.CheckSphere(transform.position, greenRange, whatIsPlayer);
        playerInYellowRange = Physics.CheckSphere(transform.position, yellowRange, whatIsPlayer);
        playerInRedRange = Physics.CheckSphere(transform.position, redRange, whatIsPlayer);

        //Checks if the player is inside the attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInGreenRange && !playerIsSprinting) Patrolling();

        if (playerInGreenRange || !playerInYellowRange
            || !playerInRedRange) Patrolling();
        //if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //if (playerInAttackRange && playerInSightRange) AttackPlayer();


    }

    void Patrolling()
    {

    }

    void ChasePlayer()
    {

    }

    void AttackPlayer()
    {

    }

    /*
    void Patroling()
    {

    }

    //void ChasePlayer()
    

    

    void AttackPlayer()
    {

    }


    
    //Once the object enabled, do this
    private void OnEnable()
    {
        ReturnToStart();
        
        
    }

    private void Start()
    {
        HeadIKSetup();
        StartCoroutine(FollowPath());
    }

    private void Update()
    {
        PerformRaycast();
        //ChasePlayer();
        
    }

    void ReturnToStart()
    {
        enemyAnimatorController.SetBool("isMoving", false);
        if (patrolWaypoints.Length > 0)
            transform.position = patrolWaypoints[0].position;
    }

    IEnumerator FollowPath()
    {

        Debug.Log("FollowPath Initiated");

        while (!isChasingPlayer)
        {
            foreach (Transform waypoint in patrolWaypoints)
            {
                yield return StartCoroutine(MoveToWaypoint(waypoint.position));
                //MoveToWaypoint(waypoint.position);
                if (!isChasingPlayer)
                    yield return new WaitForSeconds(delayBetween);
                else
                    break;
            }
        }


        
        while (!isChasingPlayer) //Added a condition to exit the loop
        {
            foreach (Transform waypoint in patrolWaypoints)
            {
                Vector3 startPosition = transform.position;
                Vector3 endPosition = waypoint.transform.position;
                float travelPercent = 0f;

                transform.LookAt(endPosition);

                while (travelPercent < 1f)
                {

                    isMoving = true;
                    Debug.Log("Moving State: " + isMoving);
                    enemyAnimatorController.SetBool("isMoving", true);
                    travelPercent += Time.deltaTime * moveSpeed;
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    yield return new WaitForEndOfFrame();

                }
                isMoving = false;
                Debug.Log("MovingState: " + isMoving);
                enemyAnimatorController.SetBool("isMoving", false);

                if (!isChasingPlayer) // Check again before the delay to ensure we don't continue if isChasingPlayer became true during the move
                    yield return new WaitForSeconds(delayBetween); // Optional delay between waypoints
                else
                    break; //exit loop naturally
            }
        }
        
        
    }


    IEnumerator MoveToWaypoint(Vector3 waypoint)
    {

        float distance = Vector3.Distance(transform.position, waypoint);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            isMoving = true;
            enemyAnimatorController.SetBool("isMoving", true);

            transform.LookAt(waypoint);
            transform.position = Vector3.MoveTowards(transform.position, waypoint, moveSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        enemyAnimatorController.SetBool("isMoving", false);


        /*
        float travelPercent = 0f;
        transform.LookAt(waypoint);

        while (travelPercent < 1f)
        {
            isMoving = true;
            enemyAnimatorController.SetBool("isMoving", true);
            travelPercent += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(transform.position, waypoint, travelPercent);
            yield return null;
        }

        isMoving = false;
        enemyAnimatorController.SetBool("isMoving", false);
        
    }

    void PerformRaycast()
    {
        Ray ray = new Ray(eyes.transform.position, eyes.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // If the ray hits something, you can check what it hit
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);

            // Here, you can implement logic to handle what to do when the ray hits something
            // For example, check if it hit the player and initiate chase behavior
            if (hit.collider.CompareTag("Player"))
            {
                isChasingPlayer = true;
                ChasePlayer();
                HeadLockOnTarget();
                Debug.Log("Player Detected! Start Chasing...");
                Debug.Log("isChasingPlayer: " + isChasingPlayer);
                // Implement logic to start chasing the player
            }
            else
            {
                isChasingPlayer = false;
                Debug.Log("Stopped Chasing Player");
                Debug.Log("isChasingPlayer: " + isChasingPlayer);
                enemyAnimatorController.SetBool("isMoving", false);
            }
        }
    }


    void ChasePlayer()
    {
        
        navMeshAgent.SetDestination(player.transform.position);
        enemyAnimatorController.SetBool("isMoving", true);

    }

    void HeadIKSetup()
    {
        multiAimConstraint = mannequinEnemy.GetComponentInChildren<MultiAimConstraint>();
        if (multiAimConstraint != null)
        {
            multiAimConstraint.weight = weight;
        }
        else
        {
            Debug.LogWarning("MultiAimConstraint not found on the specified mannequin: " + mannequinEnemy.name);
        }
    }

    void HeadLockOnTarget()
    {
        multiAimConstraint = mannequinEnemy.GetComponentInChildren<MultiAimConstraint>();

        if(multiAimConstraint = null)
        {
            Debug.Log("Multi Aim Constraint is Null!");
        }
        
        if (multiAimConstraint != null && isChasingPlayer)
        {
            
            StartCoroutine(ChangeHeadIKWeight(multiAimConstraint, weight + 1f, increaseDuration));
            Debug.Log("Weight is increasing");

        }
        if (multiAimConstraint != null && !isChasingPlayer)
        {
            StartCoroutine(ChangeHeadIKWeight(multiAimConstraint, weight - 1f, increaseDuration));
            
        }
    }

    IEnumerator ChangeHeadIKWeight(MultiAimConstraint constraint, float targetWeight, float duration)
    {
        float initialWeight = constraint.weight;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {

            constraint.weight = Mathf.Lerp(initialWeight, targetWeight, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Ensure the final weight is set exactly
        constraint.weight = targetWeight;

    }

    private void OnDrawGizmos()
    {
        // Draw the ray in the scene view for visualization
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(eyes.transform.position, eyes.transform.forward * raycastDistance);
    }

*/    
}
