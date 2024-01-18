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
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

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

    private void Update()
    {
        playerInGreenArea = Physics.CheckSphere(transform.position, greenAreaDistance, whatIsPlayer);
        playerInYellowArea = Physics.CheckSphere(transform.position, yellowAreaDistance, whatIsPlayer);
        playerInRedArea = Physics.CheckSphere(transform.position, redAreaDistance, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer);

        if(!playerInGreenArea && !playerInYellowArea && !playerInRedArea && !playerInAttackRange)
        {
            Patrolling();
        }

        if (playerInGreenArea)
        {

            //Check if the player is sprinting
            if(playerInputs != null && playerInputs.sprint)
            {
                //Player is sprinting, go to Alert state
                Alerted();
            }
            else
            {
                //Player is not sprinting, keep patrolling
                Patrolling();
            }
        }

    }

    void Patrolling()
    {
        
    }

    void Alerted()
    {

    }

    void Chase()
    {

    }

    void Attack()
    {

    }

}
