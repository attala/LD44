using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPower : MonoBehaviour
{
    public Transform PowerLevelCylinder;

    private float _currentPowerLevel = 100f;
    private float _lowestPowerCylinderPosition = -2.5f;

    public void AdjustPowerLevel(float amount)
    {
        _currentPowerLevel += amount;
        Vector3 newPosition = new Vector3(0, Mathf.Lerp(-0.95f, 1.54f, _currentPowerLevel / 100f), 0);
        PowerLevelCylinder.position = newPosition;
        if(_currentPowerLevel > 50)
            PowerLevelCylinder.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.green, (_currentPowerLevel-50f) / 100f);
        else
            PowerLevelCylinder.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, _currentPowerLevel / 50f);
    }
}
