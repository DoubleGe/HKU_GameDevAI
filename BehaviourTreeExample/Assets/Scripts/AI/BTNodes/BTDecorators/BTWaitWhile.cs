using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTWaitWhile : BTBaseNode
{
    private float waitTimeInSeconds;
    private float waitTime;
    private Func<bool> function;

    public BTWaitWhile(float waitTimeInSeconds, Func<bool> function)
    {
        this.waitTimeInSeconds = waitTimeInSeconds;
        waitTime = waitTimeInSeconds;
        this.function = function;
    }

    protected override TaskStatus OnUpdate()
    {
        waitTime = Mathf.Clamp(waitTime - Time.deltaTime, 0, waitTimeInSeconds);

        if (waitTime == 0)
        {
            waitTime = waitTimeInSeconds;
            return TaskStatus.Success;
        }

        if (!function.Invoke()) return TaskStatus.Failed;

        return TaskStatus.Running;
    }
}
