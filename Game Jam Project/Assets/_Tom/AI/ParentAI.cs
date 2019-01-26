using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAI : NavAgent
{
    private bool ReachedDestination()
    {
        if (!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
