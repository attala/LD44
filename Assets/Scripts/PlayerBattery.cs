using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattery : MonoBehaviour
{
    BatteryPower bp;
    Transform powerLevelCylinder;
    Transform tf;
    Rigidbody rb;
    bool _canShoot = false;
    float _coolDownTimer = 0.0f;

    public float MoveSpeed = 10f;
    public GameObject GameOverText;
    public GameObject ProjectilePrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        bp = GetComponent<BatteryPower>();
        powerLevelCylinder = bp.GetComponent<Transform>();
    }

    private void Update()
    {
        if (_coolDownTimer < 1)
        {
            _coolDownTimer += Time.deltaTime;
            if (_coolDownTimer >= 1)
            {
                _canShoot = true;
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (_canShoot)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        _canShoot = false;
        _coolDownTimer = 0.0f;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject p = ATTALA.PoolingManager.PM.SpawnObject(ProjectilePrefab, transform.position, Quaternion.identity);
        p.transform.LookAt(pos + (Vector3.up * 2));
    }

    void FixedUpdate()
    {
        rb.MovePosition(tf.position + new Vector3(-Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime));

        if (bp.CurrentPowerLevel <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameOverText.SetActive(true);
        Time.timeScale = 0;
    }

    void AdjustBatteryLevel(float adjustRate)
    {
        bp.AdjustPowerLevel(adjustRate * -Time.deltaTime);
    }

    public void ApplyDamage(float amount)
    {
        //Debug.Log("Applying " + amount.ToString() + " damage to player");
        bp.AdjustPowerLevel(-amount);
    }
}
