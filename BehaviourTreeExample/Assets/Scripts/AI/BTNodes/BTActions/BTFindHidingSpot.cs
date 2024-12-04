using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTFindHidingSpot : BTBaseNode
{
    private Transform transform;
    private float searchRadius;

    private Transform target;
    private string BBtargetPosition;


    public BTFindHidingSpot(Transform transform, float searchRadius, Transform target, string BBtargetPosition)
    {
        this.transform = transform;
        this.searchRadius = searchRadius;
        this.target = target;
        this.BBtargetPosition = BBtargetPosition;
    }


    protected override TaskStatus OnUpdate()
    {
        Collider[] nearbyHidingSpots = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Objects"));

        if (nearbyHidingSpots == null) return TaskStatus.Failed;
        if (nearbyHidingSpots.Length == 0) return TaskStatus.Failed;

        float closestDstSqr = float.MaxValue;
        Vector3? hidingSpot = null; //Make Vector nullable for null check later.

        for (int i = 0; i < nearbyHidingSpots.Length; i++)
        {
            Vector3 hidingPlace = CalculateHidingPlace(nearbyHidingSpots[i], target.position);

            float dstToHidingPlaceSqr = (transform.position - hidingPlace).sqrMagnitude;
            if (dstToHidingPlaceSqr < closestDstSqr)
            {
                closestDstSqr = dstToHidingPlaceSqr;
                hidingSpot = hidingPlace;
            }
        }

        if (hidingSpot == null) return TaskStatus.Failed;
        else
        {
            blackboard.SetVariable<Vector3>(BBtargetPosition, (Vector3)hidingSpot);
            return TaskStatus.Success;
        }
    }

    private Vector3 CalculateHidingPlace(Collider collider, Vector3 targetPosition)
    {
        Vector3 obstacleDir = (collider.transform.position - targetPosition).normalized;
        Vector3 pointOtherSide = collider.transform.position + obstacleDir;
        Vector3 hidingPlace = collider.ClosestPoint(pointOtherSide) + obstacleDir;

        return hidingPlace;
    }
}
