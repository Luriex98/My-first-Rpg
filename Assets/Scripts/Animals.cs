using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour
{
    private NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject Bear_4;
    public Transform player;

    public float enemyDistanceRun = 4.0f;
    public float walkPointRange;
    public bool walkPointSet;
    public Vector3 walkPoint;

    public float sightRange, attackRange;
    public bool playerInSight;

    public enum State { Patrolling, Chasing, Scared }
    private State _state = State.Patrolling;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        // playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        
        playerInSight = distance < sightRange ? true : false;
        Debug.Log(distance);

        if (playerInSight)
        {
            _state = State.Chasing;
        }
        else
        {
            _state = State.Patrolling;
            Debug.Log("Hehehe");
        }

        // Run from bear player
        if (_state != State.Scared && distance < enemyDistanceRun && Input.GetKey(KeyCode.Space))
        {
            _state = State.Scared;
        }

        switch (_state)
        {
            case State.Patrolling:
                SetColor(Color.green);
                Patroling();
                break;

            case State.Chasing:
                SetColor(Color.blue);
                ChasePlayer();
                break;

            case State.Scared:
                SetColor(Color.red);
                ScareToPlayer();
                break;
        }

    }

    private void Patroling()
    {
        if (!walkPointSet) WalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void WalkPoint()
    {
        // random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void ScareToPlayer()
    {
        Vector3 disToPlayer = transform.position - Bear_4.transform.position;

        Vector3 newPos = transform.position + disToPlayer.normalized * enemyDistanceRun;

        agent.SetDestination(newPos);
    }

    private void SetColor(Color inColor)
    {
        Renderer _renderer;
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = inColor;
    }
}