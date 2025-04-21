using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Patrulhar : MonoBehaviour
{
    private NavMeshAgent agent;
    public float range;
    private float tempo;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void Andar()
    {
        if (agent.remainingDistance <= agent.stoppingDistance || (tempo >= 0.8f))
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                agent.SetDestination(point);
                tempo = 0;
            }

            Debug.DrawLine(transform.position, agent.destination, Color.magenta);
        }
        else
        {
            tempo += Time.deltaTime;
        }
    }
}