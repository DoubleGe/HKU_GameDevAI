using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    [SerializeField] private float explosionTimeInSeconds;
    private float timer;

    [SerializeField] private GameObject smokeEffectPrefab;

    private void Start()
    {
        timer = explosionTimeInSeconds;
    }

    private void Update()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, explosionTimeInSeconds);

        if(timer == 0)
        {
            Explode();
        } 
    }

    private void Explode()
    {
        Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
