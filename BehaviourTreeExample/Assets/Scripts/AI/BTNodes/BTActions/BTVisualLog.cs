using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTVisualLog : BTBaseNode
{
    private string logText;

    public BTVisualLog(string logText)
    {
        this.logText = logText;
    }

    protected override void OnEnter()
    {
        blackboard.SetVariable<string>(logText, VariableNames.TREE_DEBUG);
    }

    protected override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
