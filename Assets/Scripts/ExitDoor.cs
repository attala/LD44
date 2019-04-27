using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private bool _powered = false;

    public Lamp lamp;
    public float MoveSpeed = 5f;
    public bool Powered { get => _powered; set => _powered = value; }

    public void PowerOn()
    {
        
        _powered = true;
    }

    public void PowerOff()
    {
        _powered = false;
    }

    private void FixedUpdate()
    {
        if (_powered)
        {
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
            if(transform.position.y > 10f)
            {
                lamp.Switch(true);
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
