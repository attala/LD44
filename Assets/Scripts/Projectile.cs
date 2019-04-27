using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectilSpeed = 40f;

    private void OnEnable()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * ProjectilSpeed * Time.deltaTime);
    }
}
