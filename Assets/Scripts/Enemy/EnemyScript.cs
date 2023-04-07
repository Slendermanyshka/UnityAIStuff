using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyScript : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float walkSpeed = 6;
    public float runSpeed = 9;

    GameObject playerGO;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask world;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPos = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerIsNear;
    bool m_InPatrol;
    bool m_CaughtPlayer;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_InPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        m_CurrentWaypointIndex = 0;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

    }
    // Update is called once per frame
    void Update()
    {
        EnviromentView();

        if (!m_InPatrol)
        {
            Chase();
        }
        else
        {
            Patrolling();
        }
    }

    
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex+1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }
    private void Chase()
    {
        m_PlayerIsNear = false;
        playerLastPos = Vector3.zero;
        if (!m_CaughtPlayer)
        {
            Move(runSpeed);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {//nyan
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, playerGO.transform.position) >= 6f)
            {
                m_InPatrol = true;
                m_PlayerIsNear = false;
                Move(walkSpeed);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                //in case of error replace playerGO with GameObject.FindGameObjectWithTag("Player")
                if (Vector3.Distance(transform.position, playerGO.transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }

        }
    }
    private void Patrolling()
    {
        if (m_PlayerIsNear)
        {
            if (m_TimeToRotate<=0)
            {
                Move(walkSpeed);
                LookingForPlayer(playerLastPos);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerIsNear = false;
            playerLastPos = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(walkSpeed);

                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }

            }
        }
    }


    void LookingForPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            m_PlayerIsNear = false;
            Move(walkSpeed);
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            m_WaitTime = startWaitTime;
            m_TimeToRotate = timeToRotate;
        }
        else
        {
            Stop();
            m_WaitTime -= Time.deltaTime;
        }
    }
    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        for (int i= 0;i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float distToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, world))
                {
                    m_PlayerInRange = true;
                    m_InPatrol = false;
                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
        
    }

}
