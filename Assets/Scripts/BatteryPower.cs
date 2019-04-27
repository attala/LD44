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
        _currentPowerLevel += amount;
        SetPLCsizeAndColor();
    }

    public void SetPowerLevel(float amount)
    {
        _currentPowerLevel = amount;
        SetPLCsizeAndColor();
    }

    void SetPLCsizeAndColor()
    {
        PowerLevelCylinder.localScale = new Vector3(PowerLevelCylinder.localScale.x, Mathf.Clamp(Mathf.Lerp(0, 1.477968f, CurrentPowerLevel / 100f), 0.01f, 1.475f), PowerLevelCylinder.localScale.z);

        if (_currentPowerLevel > 50)
        {
            PowerLevelCylinder.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.Lerp(Color.yellow, Color.green, (_currentPowerLevel - 50f) / 100f));
            PowerLevelCylinder.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(Color.yellow, Color.green, (_currentPowerLevel - 50f) / 100f));
        }   
        else
        {
            PowerLevelCylinder.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.Lerp(Color.red, Color.yellow, _currentPowerLevel / 50f));
            PowerLevelCylinder.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(Color.red, Color.yellow, _currentPowerLevel / 50f));
        }    
    }
}
