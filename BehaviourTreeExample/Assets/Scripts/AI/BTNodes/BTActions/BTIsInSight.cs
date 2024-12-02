using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsInSight : BTBaseNode
{
    private Transform transform;
    private float sightRadius;
    private string BBtargetTransform;

    public BTIsInSight(string BBtargetTransform, Transform thisTransform, float sightRadius)
    {
        this.BBtargetTransform = BBtargetTransform;
        this.transform = thisTransform;
        this.sightRadius = sightRadius;
    }

    protected override TaskStatus OnUpdate()
    {
        Transform target = blackboard.GetVariable<Transform>(BBtargetTransform);

        if (target == null) return TaskStatus.Failed;

        if (Vector3.Distance(transform.position, target.position) < 2) return TaskStatus.Success;

        Vector3 targetDir = (target.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(targetDir, transform.forward);
        float dotRad = Utility.Remap(sightRadius, -180, 0, -1, 0);

        float minDot = Mathf.Cos(sightRadius * Mathf.Deg2Rad);

        if (dotProduct < minDot) return TaskStatus.Failed;

        Debug.DrawRay(transform.position + transform.up, targetDir * 25, Color.white);
        if (Physics.Raycast(transform.position + transform.up, targetDir, out RaycastHit hit, 25, LayerMask.GetMask("Player", "Objects")))
        {
            Debug.DrawLine(transform.position + transform.up, hit.point, Color.red);
            if (hit.collider.gameObject.GetComponent<Player>())
            {
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failed;
    }
}
