using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSearchType<T> : BTBaseNode where T : MonoBehaviour
{
    private Transform transform;
    private string BBtargetPosition;
    private string BBTargetTransform;
    private float searchRadius;

    private T nearbyType;

    public BTSearchType(Transform transform, string BBtargetPosition, string BBtargetTransform, int searchRadius = 5)
    {
        this.transform = transform;
        this.BBtargetPosition = BBtargetPosition;
        this.BBTargetTransform = BBtargetTransform;
        this.searchRadius = searchRadius;
    }

    protected override void OnEnter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);

        T closestType = null;
        float dstFromType = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<T>(out T type))
            {
                float dst = Vector3.Distance(transform.position, type.transform.position);
                if (dst < dstFromType)
                {
                    closestType = type;
                    dstFromType = dst;
                }
            }
        }

        nearbyType = closestType;
    }

    protected override TaskStatus OnUpdate()
    {
        if (nearbyType != null)
        {
            blackboard.SetVariable<T>("FoundType", nearbyType);
            blackboard.SetVariable<Transform>(BBTargetTransform, nearbyType.transform);
            blackboard.SetVariable<Vector3>(BBtargetPosition, nearbyType.transform.position);
            return TaskStatus.Success;
        }
        else return TaskStatus.Failed;
    }
}
