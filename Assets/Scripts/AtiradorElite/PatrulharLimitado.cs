using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrulharLimitado : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime = 2f;

    private int currentIndex = 0;
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
        
        if (agent.remainingDistance <= agent.stoppingDistance && !esperando)
        {
            StartCoroutine(EsperarENext());
        }
    }

    IEnumerator EsperarENext()
    {
        esperando = true;

        yield return new WaitForSeconds(waitTime);

        currentIndex = (currentIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentIndex].position);
        esperando = false;
    }
}
