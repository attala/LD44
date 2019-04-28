using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    private Transform _playerTransform;
    public List<GameObject> PatrolWaypoints = new List<GameObject>();
    public float SearchRadius = 35f;
    public float CoolDownPeriod = 1f;
    public GameObject ProjectilePrefab;
    public Transform ShootPoint;
    public AudioClip AttackSound;
    public AudioSource ShootSound;

    private List<Vector3> _waypoints = new List<Vector3>();
    private bool _chasingPlayer;
    private bool _canShoot = false;
    private float _patrolSpeed;
    private float _checkDestinationTimer = 0.0f;
    private float _coolDownTimer = 0.0f;
    private int _currentWaypoint = 0;
    private float _health = 8;
    private AudioSource _audioSource;
    private AudioClip _moveSound;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _moveSound = _audioSource.clip;
        _playerTransform = GameObject.Find("PlayerBattery").transform;
        agent = GetComponent<NavMeshAgent>();
        _patrolSpeed = agent.speed;
        for (int i = 0; i < PatrolWaypoints.Count; i++)
        {
            _waypoints.Add(PatrolWaypoints[i].transform.position);
            PatrolWaypoints[i].SetActive(false);
        }
        agent.SetDestination(_waypoints[_currentWaypoint]);
    }

    
    void FixedUpdate()
    {
        if (_coolDownTimer < 1f)
        {
            _coolDownTimer += Time.deltaTime;
            if (_coolDownTimer >= 1f)
            {
                _canShoot = true;
            }
        }
        else
        {
            _canShoot = true;
        }
        _checkDestinationTimer += Time.deltaTime;
        if (_chasingPlayer)
        {
            if (_checkDestinationTimer > 0.5f)
            {
                _checkDestinationTimer = 0.0f;
                agent.SetDestination(_playerTransform.transform.position);
            }

            float distance = Vector3.Distance(transform.position, _playerTransform.transform.position);
            if (distance > 1 && _canShoot)
            {
                Shoot();
            }

            if (distance > SearchRadius * 1.4f)
            {
                _audioSource.clip = _moveSound;
                agent.speed = _patrolSpeed;
                _chasingPlayer = false;
                agent.SetDestination(_waypoints[_currentWaypoint]);
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, _waypoints[_currentWaypoint]);
            if (distance < 1)
            {
                _currentWaypoint++;
                if (_currentWaypoint > _waypoints.Count -1)
                {
                    _currentWaypoint = 0;
                }
                agent.SetDestination(_waypoints[_currentWaypoint]);
            }
            if (_checkDestinationTimer > 1f)
            {
                _checkDestinationTimer = 0.0f;
                CheckForTargets();
            }
        }
    }

    private void Shoot()
    {
        ShootSound.Play();
        _canShoot = false;
        _coolDownTimer = 0.0f;
        //Vector3 dir = (_playerTransform.position - transform.position).normalized;
        GameObject p = ATTALA.PoolingManager.PM.SpawnObject(ProjectilePrefab, ShootPoint.position, Quaternion.identity);
        p.transform.LookAt(_playerTransform.position + (Vector3.up *2));
    }

    void CheckForTargets()
    {
        float distance = Vector3.Distance(_playerTransform.position, transform.position);

        if(distance < SearchRadius)
        {
            Vector3 dir = ((_playerTransform.position + (Vector3.up * 2)) - ShootPoint.position).normalized;
            Debug.DrawRay(ShootPoint.position, dir * 100, Color.green);

            RaycastHit objectHit;
            if (Physics.Raycast(ShootPoint.position, dir, out objectHit, 100))
            {
                //do something if hit object ie
                if (objectHit.transform.tag == "Player")
                {
                    _audioSource.clip = AttackSound;
                    agent.SetDestination(_playerTransform.position);
                    agent.speed *= 2.2f;
                    _chasingPlayer = true;
                }
            }
        }
    }

        
    

    public void ApplyDamage(float amount)
    {
        _health -= amount;
        if (_health < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
