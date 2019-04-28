using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private bool _powered = false;
    private AudioSource _audioSource;

    public Lamp lamp;
    public float MoveSpeed = 5f;
    public bool Powered { get => _powered; set => _powered = value; }
    public AudioClip GreenLightSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PowerOn()
    {
        _powered = true;
        _audioSource.Play();
    }

    public void PowerOff()
    {
        _powered = false;
        _audioSource.Stop();
    }

    private void FixedUpdate()
    {
        if (_powered)
        {
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
            if(transform.position.y > 10f)
            {
                if (!lamp.On)
                {
                    _audioSource.clip = GreenLightSound;
                    _audioSource.loop = false;
                    _audioSource.Play();
                    lamp.Switch(true);
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }
}
