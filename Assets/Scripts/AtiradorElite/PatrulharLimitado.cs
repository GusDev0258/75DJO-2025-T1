using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrulharLimitado : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime;

    [HideInInspector]
    public int currentIndex = 0;
    private NavMeshAgent agent;

    private bool esperando = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;
        if (!esperando && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    StartCoroutine(EsperarENext());
                }
            }
        }
    }

    private IEnumerator EsperarENext()
    {
        esperando = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(0.2f);

        currentIndex = (currentIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentIndex].position);
        esperando = false;
        agent.isStopped = false;
    }
}