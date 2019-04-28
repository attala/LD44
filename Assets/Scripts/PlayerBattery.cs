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
    private AudioClip _shootSound;

    public bool FlipHorizontalAndVertical = false;
    public bool FlipHorizontal = false;
    public bool FlipVertical = false;
    public float MoveSpeed = 10f;
    public GameObject GameOverText;
    public GameObject ProjectilePrefab;
    public Camera Cam;
    public bool HasWeapon = true;
    public AudioClip NoAmmoSound;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _shootSound = _audioSource.clip;
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
        if (bp.CurrentPowerLevel > 10)
        {
            bp.AdjustPowerLevel(-2f);
            _audioSource.clip = _shootSound;
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
            p.transform.LookAt(new Vector3(point.x, transform.position.y + 2, point.z));
        }
        else
        {
            _audioSource.clip = NoAmmoSound;
            _audioSource.Play();
        }
    }

    void FixedUpdate()
    {
        float inputVertical = -Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        if (FlipHorizontal)
        {
            inputHorizontal = -Input.GetAxis("Horizontal");
        }
        if (FlipVertical)
        {
            inputVertical = Input.GetAxis("Vertical");
        }

        if (FlipHorizontalAndVertical)
        {
            rb.MovePosition(tf.position + new Vector3(inputHorizontal * MoveSpeed * Time.deltaTime, 0, inputVertical * MoveSpeed * Time.deltaTime));
        }
        else
        {
            rb.MovePosition(tf.position + new Vector3(inputVertical * MoveSpeed * Time.deltaTime, 0, inputHorizontal * MoveSpeed * Time.deltaTime));
        }
        

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
