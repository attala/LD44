using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltMovement : MonoBehaviour
{
    private List<Transform> _objectsOnBelt = new List<Transform>();
    private bool _powered = true;
    public float BeltSpeed = 10f;

    public bool Powered { get => _powered; set => _powered = value; }

    private void OnTriggerEnter(Collider other)
    {
        _objectsOnBelt.Add(other.transform);
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
            for (int i = 0; i < _objectsOnBelt.Count; i++)
            {
                _objectsOnBelt[i].Translate(transform.forward * BeltSpeed * Time.deltaTime);
            }
        }
    }
}
