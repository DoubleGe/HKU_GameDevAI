﻿using System.Collections;
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

    [SerializeField] private SmokeGrenade smokeGrenadePrefab;

    //$$TEMP Guard Ref
    private Transform tempGuard;

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
        blackboard.SetVariable<string>(VariableNames.TREE_DEBUG, "");

        tree =
            new BTSelector(
                new BTSequence(
                    new BTSearchType<Guard>(transform, VariableNames.ROGUE_GUARD_POSITION, VariableNames.TARGET_TRANSFORM),
                    new BTRepeatUntil(
                        new BTSequence(
                            new BTVisualLog("Going to hiding spot"),
                            new BTFindHidingSpot(transform, 7, tempGuard, VariableNames.TARGET_POSITION),
                            new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance, .7f),
                            new BTWait(2),
                            new BTSearchType<Guard>(transform, VariableNames.ROGUE_GUARD_POSITION, VariableNames.TARGET_TRANSFORM, 10),
                            new BTVisualLog("Throw Grenade"),
                            new BTThrowObject<SmokeGrenade>(smokeGrenadePrefab, transform, VariableNames.ROGUE_GUARD_POSITION)
                    ), () => GlobalData.Instance.globalBlackboard.GetVariable<bool>(GlobalVariableNames.GUARD_SEES_PLAYER))
                ),
                new BTSequence(
                    new BTVisualLog("Going to player"),
                    new BTGetTargetPosition(player.transform, VariableNames.TARGET_POSITION),
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
