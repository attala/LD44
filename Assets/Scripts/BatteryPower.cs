using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPower : MonoBehaviour
{
    public Transform PowerLevelCylinder;
    public float StartPower = 100f;

    private float _currentPowerLevel = 100f;

    public float CurrentPowerLevel { get => _currentPowerLevel; set => _currentPowerLevel = value; }

    private void Start()
    {
        SetPowerLevel(StartPower);
    }

    public void AdjustPowerLevel(float amount)
    {
        CurrentPowerLevel += amount;
        SetPLCpositionAndColor();
    }

    public void SetPowerLevel(float amount)
    {
        CurrentPowerLevel = amount;
        SetPLCpositionAndColor();
    }

    void SetPLCpositionAndColor()
    {
        Vector3 newPosition = new Vector3(0, Mathf.Lerp(-2.44f, 0, CurrentPowerLevel / 100f), 0);
        PowerLevelCylinder.localPosition = newPosition;
        if (CurrentPowerLevel > 50)
            PowerLevelCylinder.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.green, (CurrentPowerLevel - 50f) / 100f);
        else
            PowerLevelCylinder.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, CurrentPowerLevel / 50f);
    }
}
