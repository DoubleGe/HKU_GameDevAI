using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StateDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateDisplay;
    private Blackboard agentBlackboard;

    private void Update()
    {
        if (agentBlackboard == null) return;

        stateDisplay.text = agentBlackboard.GetVariable<string>(VariableNames.TREE_DEBUG);
    }

    public void SetBlackboard(Blackboard blackboard)
    {
        agentBlackboard = blackboard;
    }
}
