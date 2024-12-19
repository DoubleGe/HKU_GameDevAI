using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTStopAgent : BTBaseNode
{
    private NavMeshAgent agent;

    public BTStopAgent(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    protected override void OnEnter()
    {
        agent.isStopped = true;
    }

    protected override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
