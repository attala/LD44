using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectilSpeed = 40f;
    public GameObject HitPrefab;

    float _lifeTimer = 0.0f;

    private void OnEnable()
    {
        _lifeTimer = 0.0f;
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * ProjectilSpeed * Time.deltaTime, Space.World);

        _lifeTimer += Time.deltaTime;

        if (_lifeTimer > 4)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("ApplyDamage", Random.Range(2f, 5f), SendMessageOptions.DontRequireReceiver);
        ATTALA.PoolingManager.PM.SpawnParticleObject(null, HitPrefab, transform.position, Quaternion.identity, true);
        gameObject.SetActive(false);
    }
}
