using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTAttack : BTBaseNode
{
    private Transform transform;
    private Weapon weapon;

    public BTAttack(Transform transform, Weapon weapon)
    {
        this.transform = transform;
        this.weapon = weapon;
    }

    protected override void OnEnter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(transform.gameObject, 1);
            }
        }
    }

    protected override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
