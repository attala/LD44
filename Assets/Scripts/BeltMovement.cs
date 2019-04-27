using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltMovement : MonoBehaviour
{
    private List<Transform> _objectsOnBelt = new List<Transform>();
    private bool _powered = false;
    private Renderer renderer;
    private float _beltTexOffset = 0.0f;
    private AudioSource _beltSound;

    public float BeltSpeed = 5f;
    public bool Powered { get => _powered; set => _powered = value; }
    

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        _beltSound = transform.root.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChargeAble")
        {
            _objectsOnBelt.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int toremove = -1;
        for (int i = 0; i < _objectsOnBelt.Count; i++)
        {
            if (other.transform == _objectsOnBelt[i])
            {
                toremove = i;
            }
        }
        if (toremove != -1)
        {
            _objectsOnBelt.RemoveAt(toremove);
        }
        else
        {
            Debug.LogError("Trying to remove an object from the conveyor belt that doesn't exist!");
        }
    }

    void FixedUpdate()
    {
        if (Powered)
        {
            if (!_beltSound.isPlaying)
            {
                _beltSound.Play();
            }
            float offset = Time.time * 1.95f;
            renderer.material.mainTextureOffset = new Vector2(0, offset);
            for (int i = 0; i < _objectsOnBelt.Count; i++)
            {
                _objectsOnBelt[i].Translate(transform.forward * BeltSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (_beltSound.isPlaying)
            {
                _beltSound.Stop();
            }
        }
    }
}
