using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rogue : MonoBehaviour
{

    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //Create your Behaviour Tree here!
        Blackboard blackboard = new Blackboard();
        blackboard.SetVariable(VariableNames.ENEMY_HEALTH, 100);
        blackboard.SetVariable(VariableNames.TARGET_POSITION, new Vector3(0, 0, 0));
        blackboard.SetVariable(VariableNames.CURRENT_PATROL_INDEX, -1);

        tree =
            new BTSelector(
                
            );

        tree.SetupBlackboard(blackboard);
    }

    private void FixedUpdate()
    {
        tree?.Tick();
    }
}
