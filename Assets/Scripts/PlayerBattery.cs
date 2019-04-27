using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattery : MonoBehaviour
{
    BatteryPower bp;
    Transform powerLevelCylinder;
    Transform tf;
    Rigidbody rb;
    public float MoveSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        bp = GetComponent<BatteryPower>();
        powerLevelCylinder = bp.GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        //tf.Translate(-Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, 0, -Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime);
        rb.MovePosition(tf.position + new Vector3(-Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime));
    }

    void AdjustBatteryLevel(float adjustRate)
    {
        bp.AdjustPowerLevel(adjustRate * -Time.deltaTime);
    }
}
