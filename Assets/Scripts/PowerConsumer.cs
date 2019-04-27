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

    bool _poweredTriggered = false, _unPoweredTriggered = false;

    private void Start()
    {
        recharger = GetComponent<Recharger>();
    }

    private void FixedUpdate()
    {
        if (recharger.Battery_2.CurrentPowerLevel > 1)
        {
            recharger.Battery_2.GetComponent<BatteryPower>().AdjustPowerLevel(-PowerConsumptionRate * Time.deltaTime);
            if (!_poweredTriggered)
            {
                _poweredTriggered = true;
                _unPoweredTriggered = false;
                Powered.Invoke();
            } 
        }

        if (recharger.Battery_2.CurrentPowerLevel < 1)
        {
            if (!_unPoweredTriggered)
            {
                _poweredTriggered = false;
                _unPoweredTriggered = true;
                UnPowered.Invoke();
            }
        }
    }
}
