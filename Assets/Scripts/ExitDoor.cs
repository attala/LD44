using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private bool _powered = false;
    private AudioSource _audioSource;
    private float _startHeight;

    public Lamp lamp;
    public float MoveSpeed = 5f;
    public bool Powered { get => _powered; set => _powered = value; }
    public AudioClip GreenLightSound;
    public float OpenHeight = 10f;

    private void Start()
    {
        _startHeight = transform.position.y;
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
            if (transform.position.y < _startHeight + OpenHeight)
            {
                transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
            }
            else
            {
                //If it's an exit door
                if (lamp)
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
}
