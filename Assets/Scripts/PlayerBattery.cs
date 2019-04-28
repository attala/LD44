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
    private AudioSource _audioSource;

    public float MoveSpeed = 10f;
    public GameObject GameOverText;
    public GameObject ProjectilePrefab;
    public Camera Cam;
    public bool HasWeapon = true;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
            if (_canShoot && HasWeapon)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        _audioSource.Play();
        _canShoot = false;
        _coolDownTimer = 0.0f;
        Vector3 point = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    point = hit.point;
                }
            }
        GameObject p = ATTALA.PoolingManager.PM.SpawnObject(ProjectilePrefab, transform.position + (Vector3.up * 2) + ((point - transform.position).normalized * 2), Quaternion.identity);
        p.transform.LookAt(point + (Vector3.up * 2));
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
