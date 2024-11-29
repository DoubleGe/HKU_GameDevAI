using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPickupWeapon : BTBaseNode
{
    private Transform transform;
    private string BBweaponStorage;

    public BTPickupWeapon(Transform transform, string BBweaponStorage)
    {
        this.transform = transform;
        this.BBweaponStorage = BBweaponStorage;
    }

    protected override TaskStatus OnUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

        Weapon closestWeapon = null;
        float dstFromType = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Weapon>(out Weapon type))
            {
                float dst = Vector3.Distance(transform.position, type.transform.position);
                if (dst < dstFromType)
                {
                    closestWeapon = type;
                    dstFromType = dst;
                }
            }
        }

        if(closestWeapon != null)
        {
            closestWeapon.transform.SetParent(transform, true);
            closestWeapon.transform.localPosition = Vector3.zero;
            blackboard.SetVariable<Weapon>(BBweaponStorage, closestWeapon);
            return TaskStatus.Success;
        } else return TaskStatus.Failed;
       
    }
}
