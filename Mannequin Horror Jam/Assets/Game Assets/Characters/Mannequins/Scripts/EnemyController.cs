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

    [Header("Patrol Waypoints")]
    [Tooltip("Assign Patrol Waypoints Here")]
    public Transform[] patrolWaypoints;
    public int currentWaypointIndex;

    [Header("States and Distances for AI")]
    public float greenAreaDistance;
    public float yellowAreaDistance;
    public float redAreaDistance;
    public float attackDistance;

    public bool playerInGreenArea;
    public bool playerInYellowArea;
    public bool playerInRedArea;
    public bool playerInAttackRange;

    private StarterAssetsInputs playerInputs;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        currentWaypointIndex = 0;

        playerInputs = player.GetComponent<StarterAssetsInputs>();

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
