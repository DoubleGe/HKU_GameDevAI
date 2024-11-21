using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInvert : BTDecorator
{
    public BTInvert(BTBaseNode child) : base(child)
    {
    }

    protected override TaskStatus OnUpdate()
    {
        if (child.Tick() == TaskStatus.Failed) return TaskStatus.Success;
        else if (child.Tick() == TaskStatus.Success) return TaskStatus.Failed;
        else return child.Tick();
    }
}
