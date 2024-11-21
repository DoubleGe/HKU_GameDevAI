using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BTParallel : BTComposite
{
    private int succeedCount;

    public BTParallel(int succeedCount, params BTBaseNode[] children) : base(children)
    {
        if (succeedCount > children.Length)
        {
            Debug.LogWarning("Succeed Count is higher than the child count. Setting it to the child count.");
            succeedCount = children.Length;
        }

        this.succeedCount = succeedCount;
    }
    protected override TaskStatus OnUpdate()
    {
        TaskStatus[] statuses = new TaskStatus[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            statuses[i] = children[i].Tick();
        }

        if (statuses.ToList().FindAll(s => s == TaskStatus.Success).Count >= succeedCount) return TaskStatus.Success;
        else if (statuses.ToList().FindAll(s => s == TaskStatus.Failed).Count > children.Length - succeedCount) return TaskStatus.Failed;
        else return TaskStatus.Running;
    }
}
