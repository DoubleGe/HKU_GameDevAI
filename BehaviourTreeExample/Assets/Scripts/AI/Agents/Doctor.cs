using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Doctor : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform waitPosition;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float keepDistance = 1f;
    [SerializeField] private float playerKeepDistance = 4f;

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
        blackboard.SetVariable<string>(VariableNames.TREE_DEBUG, "");

        tree =
            new BTSelector(
                new BTConditional(
                    new BTSequence(
                        new BTVisualLog("Moving to player"),
                        new BTGetTargetPosition(player.transform, VariableNames.TARGET_POSITION),
                        new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, playerKeepDistance),
                        new BTVisualLog("Reviving player"),
                        new BTWait(3),
                        new BTFunction(() => player.RevivePlayer())
                ), () => GlobalData.Instance.globalBlackboard.GetVariable<bool>(GlobalVariableNames.PLAYER_IS_DEAD)),

                new BTSequence(
                    new BTVisualLog("Going to base"),
                    new BTGetTargetPosition(waitPosition, VariableNames.TARGET_POSITION),
                    new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, playerKeepDistance)
                )
            );

        blackboard.SetVariable<BTBaseNode>(VariableNames.TREE_DEBUG, tree);

        tree.SetupBlackboard(blackboard);

        if (TryGetComponent<StateDebug>(out StateDebug debug)) debug.SetBlackboard(blackboard);
    }

    private void FixedUpdate()
    {
        tree?.Tick();
    }
}
