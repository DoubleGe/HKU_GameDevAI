using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTConditional : BTDecorator
{
    private Func<bool> condition;

    public BTConditional(BTBaseNode child, Func<bool> condition) : base(child)
    {
        this.condition = condition;
    }

    protected override TaskStatus OnUpdate()
    {
        if (condition.Invoke())
        {
            return child.Tick();
        } else return TaskStatus.Failed;
    }
}
