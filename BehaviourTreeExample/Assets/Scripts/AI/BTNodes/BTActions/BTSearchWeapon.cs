using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSearchWeapon : BTBaseNode
{
    private Transform transform;
    private string BBtargetPosition;

    private Weapon nearbyWeapon;

    public BTSearchWeapon(Transform transform, string BBtargetPosition)
    {
        this.transform = transform;
        this.BBtargetPosition = BBtargetPosition;
    }

    protected override void OnEnter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

        Weapon closestWeapon = null;
        float dstFromWeapon = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent<Weapon>(out Weapon weapon))
            {
                float dst = Vector3.Distance(transform.position, weapon.transform.position);
                if (dst < dstFromWeapon)
                {
                    closestWeapon = weapon;
                    dstFromWeapon = dst;
                }
            }
        }

        nearbyWeapon = closestWeapon;
    }

    protected override TaskStatus OnUpdate()
    {
        if (nearbyWeapon != null)
        {
            blackboard.SetVariable<Vector3>(BBtargetPosition, nearbyWeapon.transform.position);
            return TaskStatus.Success;
        }
        else return TaskStatus.Failed;
    }
}
