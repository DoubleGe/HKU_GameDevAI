using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<ISmokeable>(out ISmokeable smokeable))
            {
                smokeable.SmokeTarget();
            }
        }
    }
}

//Should have moved this to a proper class.
public interface ISmokeable
{
    public void SmokeTarget();
}