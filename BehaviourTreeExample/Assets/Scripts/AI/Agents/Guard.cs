using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public float moveSpeed = 3;
    public float keepDistance = 1f;
    public Transform[] wayPoints;
    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private Transform guardHand;

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

        tree =
            new BTSelector(
                new BTSequence(
                    new BTSearchType<Player>(transform, VariableNames.PLAYER_POSITION, VariableNames.TARGET_TRANSFORM),
                    new BTIsInSight(VariableNames.TARGET_TRANSFORM, transform, 130),
                    new BTSelector(
                        new BTConditional(
                            new BTSequence(
                                new BTMoveToPosition(agent, moveSpeed, VariableNames.PLAYER_POSITION, keepDistance),
                                new BTWaitWhile(2, () => Vector3.Distance(blackboard.GetVariable<Transform>(VariableNames.TARGET_TRANSFORM).position, transform.position) <= 1.5f),
                                new BTAttack(transform, blackboard.GetVariable<Weapon>(VariableNames.WEAPON_STORAGE))
                        ), () => blackboard.ContainsValue<Weapon>(VariableNames.WEAPON_STORAGE)),

                        new BTSequence(
                            new BTSearchType<Weapon>(transform, VariableNames.TARGET_POSITION, VariableNames.TARGET_TRANSFORM),
                            new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance, .75f),
                            new BTPickupWeapon(guardHand, VariableNames.WEAPON_STORAGE)
                        )
                    )
                ),

                new BTRepeater(wayPoints.Length,
                    new BTSequence(
                        new BTGetNextPatrolPosition(wayPoints),
                        new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance)
                    )
                )
            );

        tree.SetupBlackboard(blackboard);
    }

    private void FixedUpdate()
    {
        TaskStatus result = tree.Tick();
    }
}
