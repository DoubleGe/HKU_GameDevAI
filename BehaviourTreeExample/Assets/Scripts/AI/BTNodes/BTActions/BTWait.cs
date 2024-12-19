using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTWait : BTBaseNode
{
    private float waitTimeInSeconds;
    private float waitTime;
    public BTWait(float waitTimeInSeconds)
    {
        this.waitTimeInSeconds = waitTimeInSeconds;
        waitTime = waitTimeInSeconds;
    }

    protected override void OnEnter()
    {
        waitTime = waitTimeInSeconds;
    }

    protected override void OnExit()
    {
        waitTime = waitTimeInSeconds;
    }

    protected override TaskStatus OnUpdate()
    {
        waitTime = Mathf.Clamp(waitTime - Time.deltaTime, 0, waitTimeInSeconds);
 
        if(waitTime == 0)
        {
            waitTime = waitTimeInSeconds;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
