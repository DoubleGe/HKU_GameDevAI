using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLog : BTBaseNode
{
    private object log;

    public BTLog(object log)
    {
        this.log = log;
    }

    protected override TaskStatus OnUpdate()
    {
        Debug.Log(log);
        return TaskStatus.Success;  
    }
}
