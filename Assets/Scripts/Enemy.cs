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

    private List<Vector3> _waypoints = new List<Vector3>();
    private bool _chasingPlayer;
    private bool _canShoot = false;
    private float _patrolSpeed;
    private float _checkDestinationTimer = 0.0f;
    private float _coolDownTimer = 0.0f;
    private int _currentWaypoint = 0;

    void Awake()
    {
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

            if (distance > 35)
            {
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
        _canShoot = false;
        _coolDownTimer = 0.0f;
        Vector3 dir = (_playerTransform.position - transform.position).normalized;
        GameObject p = ATTALA.PoolingManager.PM.SpawnObject(ProjectilePrefab, transform.position, Quaternion.identity);
        p.transform.rotation.SetLookRotation(dir, Vector3.up);
    }

    void CheckForTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SearchRadius);
        if (hitColliders.Length != 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "Player")
                {
                    agent.SetDestination(hitColliders[i].transform.position);
                    agent.speed *= 2.2f;
                    _chasingPlayer = true;
                }
            }
        }
    }
}
