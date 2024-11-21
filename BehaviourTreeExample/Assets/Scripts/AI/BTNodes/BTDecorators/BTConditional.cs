using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTConditional : BTDecorator
{
    public BTConditional(Func<bool> condition, BTBaseNode child, BTBaseNode child2) : base(child)
    {
        
    }

    protected override TaskStatus OnUpdate()
    {
        return TaskStatus.Failed;
    }
}
