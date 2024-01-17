using System.Collections;
using System.Collections.Generic;
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
    [Tooltip("Distance for eyes to see")]
    [SerializeField] float raycastDistance;
    [Tooltip("Assign the speed for the enemy")]
    [SerializeField] float moveSpeed;
    [Tooltip("Delay between waypoints")]
    [SerializeField] float delayBetween;
    [Tooltip("Float variable for head turn DO NOT CHANGE")]
    [SerializeField] float weight;
    [Tooltip("Head turn weight increase duration")]
    [SerializeField] float increaseDuration = 3f;
    [Tooltip("This is for debugging and animation triggering")]
    [SerializeField] bool isMoving;
    [Tooltip("This is for debugging and animation triggering")]
    [SerializeField] bool isChasingPlayer;


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
        ChasePlayer();
        
    }

    void ReturnToStart()
    {
        enemyAnimatorController.SetBool("isMoving", false);
        if (patrolWaypoints.Length > 0)
            transform.position = patrolWaypoints[0].position;
    }

    IEnumerator FollowPath()
    {

        while (!isChasingPlayer) //Added a considiton to exit the loop
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

                if (isChasingPlayer) StopCoroutine(FollowPath());
            }
        }

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
                HeadLockOnTarget();
                Debug.Log("Player Detected! Start Chasing...");
                // Implement logic to start chasing the player
            }
            else
            {
                isChasingPlayer = false;
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

        if (multiAimConstraint != null)
        {
            
            StartCoroutine(IncreaseWeightGradually(multiAimConstraint, weight + 1f, increaseDuration));
            Debug.Log("Weight is increasing");

        }
        else
        {
            Debug.Log("MultiAim Constraint is null");
        }
    }

    IEnumerator IncreaseWeightGradually(MultiAimConstraint constraint, float targetWeight, float duration)
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

}
