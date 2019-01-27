using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgent : MonoBehaviour
{
    protected NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        print(agent);
    }

    protected void MoveTo(Vector3 position)
    {
        if (agent)
        {
            agent.SetDestination(position);
        }
        else
        {
            Debug.Log("No agent :<");
        }
    }

    protected bool ReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}