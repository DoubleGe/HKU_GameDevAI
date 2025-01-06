using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BTThrowObject<T> : BTBaseNode where T : MonoBehaviour
{
    private T prefab;
    private string BBtargetPosition;
    private float angle = 45;
    private Transform transform;

    private bool succesfull;

    public BTThrowObject(T prefab, Transform transform, string BBtargerPosition)
    {
        this.prefab = prefab;
        this.transform = transform;
        this.BBtargetPosition = BBtargerPosition;
    }

    protected override void OnEnter()
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 1.8f; 

        T tempPrefab = GameObject.Instantiate(prefab, spawnPosition, transform.rotation);
        Rigidbody rb = tempPrefab.GetComponent<Rigidbody>();

        Vector3 targetPosition = blackboard.GetVariable<Vector3>(BBtargetPosition);

        Vector3 direction = targetPosition - rb.position;
        float dst = direction.magnitude;

        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z).normalized;

        float gravity = Physics.gravity.y;
        float heightDiff = direction.y;

        float angleRad = angle * Mathf.Deg2Rad;

        float velocitySquared = (gravity * dst * dst) / (2 * (heightDiff - Mathf.Tan(angleRad) * dst) * Mathf.Pow(Mathf.Cos(angleRad), 2));
        if (velocitySquared <= 0) return;

        float velocity = Mathf.Sqrt(velocitySquared);

        Vector3 velocityVector = directionXZ * velocity * Mathf.Cos(angleRad);
        velocityVector.y = velocity * Mathf.Sin(angleRad);

        rb.velocity = velocityVector;
        succesfull = true;
    }

    protected override TaskStatus OnUpdate()
    {
        if (succesfull) return TaskStatus.Success;
        else return TaskStatus.Failed;
    }

    protected override void OnExit()
    {
        succesfull = false;
    }
}
