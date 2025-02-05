﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour, ISmokeable
{
    public float moveSpeed = 3;
    public float keepDistance = 1f;
    public Transform[] wayPoints;
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private Transform guardHand;

    [SerializeField] private bool inSmoke;
    private float smokeCooldown;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Blackboard blackboard = new Blackboard();
        blackboard.SetVariable(VariableNames.ENEMY_HEALTH, 100);
        blackboard.SetVariable(VariableNames.TARGET_POSITION, new Vector3(0, 0, 0));
        blackboard.SetVariable(VariableNames.CURRENT_PATROL_INDEX, -1);
        blackboard.SetVariable<Transform>(VariableNames.TARGET_TRANSFORM, null);
        blackboard.SetVariable<string>(VariableNames.TREE_DEBUG, "");

        tree =
            new BTSelector(
                new BTConditional(
                    new BTSequence(
                        new BTVisualLog("In Smoke"),
                        new BTFunction(() => agent.isStopped = true),
                        new BTWait(3f),
                        new BTFunction(() => agent.isStopped = false),
                        new BTFunction(() => inSmoke = false)
                ), () => inSmoke),

                new BTSequence(
                    new BTSearchType<Player>(transform, VariableNames.PLAYER_POSITION, VariableNames.TARGET_TRANSFORM),
                    new BTIsInSight(VariableNames.TARGET_TRANSFORM, transform, 130),
                     new BTFunction(() => GlobalData.Instance.GuardSeesPlayer(this)),
                    new BTSelector(
                        new BTConditional(
                            new BTSequence(
                                new BTVisualLog("Moving to player"),
                                new BTMoveToPosition(agent, moveSpeed, VariableNames.PLAYER_POSITION, keepDistance),
                                new BTVisualLog("Attacking player"),
                                new BTWaitWhile(2, () => Vector3.Distance(blackboard.GetVariable<Transform>(VariableNames.TARGET_TRANSFORM).position, transform.position) <= 1.5f),
                                new BTAttack(transform, blackboard.GetVariable<Weapon>(VariableNames.WEAPON_STORAGE))
                        ), () => blackboard.ContainsValue<Weapon>(VariableNames.WEAPON_STORAGE)),

                        new BTConditional(
                            new BTSequence(
                                new BTVisualLog("Walking to weapon"),
                                new BTSearchType<Weapon>(transform, VariableNames.TARGET_POSITION, VariableNames.TARGET_TRANSFORM),
                                new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance, .75f),
                                new BTPickupWeapon(guardHand, VariableNames.WEAPON_STORAGE)
                            ), () => !blackboard.ContainsValue<Weapon>(VariableNames.WEAPON_STORAGE)
                        )
                    )
                ),

                new BTRepeatUntil(
                    new BTSequence(
                        new BTVisualLog("Walking to waypoint"),
                        new BTFunction(() => GlobalData.Instance.GuardLostPlayer(this)),
                        new BTGetNextPatrolPosition(wayPoints),
                        new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance)
                        )
                , () => !inSmoke)
            );

        tree.SetupBlackboard(blackboard);

        if (TryGetComponent<StateDebug>(out StateDebug debug)) debug.SetBlackboard(blackboard);
    }

    private void Update()
    {
        smokeCooldown = Mathf.Max(smokeCooldown - Time.deltaTime, 0);
    }

    private void FixedUpdate()
    {
        TaskStatus result = tree.Tick();
    }

    public void SmokeTarget()
    {
        if (smokeCooldown == 0)
        {
            inSmoke = true;
            smokeCooldown = 6;
        }
    }
}
