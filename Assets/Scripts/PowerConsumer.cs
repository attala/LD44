using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerConsumer : MonoBehaviour
{
    private Recharger recharger;
    public float PowerConsumptionRate = 5f;
    public UnityEvent Powered;
    public UnityEvent UnPowered;

    private Lamp _lamp;
    bool _poweredTriggered = false, _unPoweredTriggered = false;

    private void Start()
    {
        _lamp = GetComponentInChildren<Lamp>();
        recharger = GetComponent<Recharger>();
    }

    private void FixedUpdate()
    {
        if (recharger.Battery_2.CurrentPowerLevel > 1)
        {
            recharger.Battery_2.GetComponent<BatteryPower>().AdjustPowerLevel(-PowerConsumptionRate * Time.deltaTime);
            if (!_poweredTriggered)
            {
                _lamp.Switch(true);
                _poweredTriggered = true;
                _unPoweredTriggered = false;
                Powered.Invoke();
            } 
        }

        if (recharger.Battery_2.CurrentPowerLevel < 1)
        {
            if (!_unPoweredTriggered)
            {
                _lamp.Switch(false);
                _poweredTriggered = false;
                _unPoweredTriggered = true;
                UnPowered.Invoke();
            }
        }
    }
}
