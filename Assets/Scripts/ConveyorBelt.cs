using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    BeltMovement _bm;

    void Start()
    {
        _bm = GetComponentInChildren<BeltMovement>();
    }

    void FixedUpdate()
    {
     
    }
}
