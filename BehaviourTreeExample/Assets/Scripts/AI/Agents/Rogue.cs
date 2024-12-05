using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Rogue : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float keepDistance = 1f;
    [SerializeField] private float playerKeepDistance = 4f;

    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;

    //$$TEMP Guard Ref
    private Transform tempGuard;
    private float panicTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        //$$TEMP Guard Ref
        tempGuard = FindObjectOfType<Guard>().transform;
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
                new BTSequence(
                    new BTSearchType<Guard>(transform, VariableNames.TARGET_POSITION, VariableNames.TARGET_TRANSFORM),
                    new BTFunction(() => panicTimer = 5),
                    new BTRepeatUntil(
                        new BTSequence(
                            new BTFindHidingSpot(transform, 7, tempGuard, VariableNames.TARGET_POSITION),
                            new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance, .7f)
                    ), () => panicTimer > 0)
                ),
                new BTSequence(
                    new BTGetTargetPosition(player.transform, VariableNames.TARGET_POSITION),
                    new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, playerKeepDistance)
                )
            );

        tree.SetupBlackboard(blackboard);
    }

    private void Update()
    {
        panicTimer = Mathf.Max(panicTimer - Time.deltaTime, 0);
    }

    private void FixedUpdate()
    {
        tree?.Tick();
    }
}
