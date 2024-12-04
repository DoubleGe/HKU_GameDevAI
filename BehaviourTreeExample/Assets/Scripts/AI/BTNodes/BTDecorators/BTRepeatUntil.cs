using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRepeatUntil : BTDecorator
{
    private Func<bool> untilFunc;
    
    public BTRepeatUntil(BTBaseNode child, Func<bool> untilFunc) : base(child) { this.untilFunc = untilFunc; }

    protected override TaskStatus OnUpdate()
    {
        if (untilFunc.Invoke())
        {
            child.Tick();
            return TaskStatus.Running;
        }
        else return TaskStatus.Failed;
    }
}
