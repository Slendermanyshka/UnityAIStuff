using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyScript : MonoBehaviour
{

    public Transform target;
    public float UpdateSpeed = 0.1f;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartCoroutine(FollowTartget());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FollowTartget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateSpeed);
        while (enabled)
        {
            agent.SetDestination(target.transform.position);
            yield return Wait;
        }
    }
}
