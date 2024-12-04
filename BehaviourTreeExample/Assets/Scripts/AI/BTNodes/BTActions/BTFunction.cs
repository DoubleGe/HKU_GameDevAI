using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTFunction : BTBaseNode
{
    private Action action;

    public BTFunction(Action action)
    {
        this.action = action;
    }

    protected override TaskStatus OnUpdate()
    {
        action.Invoke();
        return TaskStatus.Success;
    }
}
