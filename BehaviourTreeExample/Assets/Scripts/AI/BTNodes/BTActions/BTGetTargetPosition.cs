using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTGetTargetPosition : BTBaseNode
{
    private Transform target;
    private string BBtargetPosition;

    public BTGetTargetPosition(Transform target, string BBtargetPosition)
    {
        this.target = target;
        this.BBtargetPosition = BBtargetPosition;
    }

    protected override TaskStatus OnUpdate()
    {
        blackboard.SetVariable<Vector3>(BBtargetPosition, target.position);
        return TaskStatus.Success;
    }
}
