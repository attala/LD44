using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recharger : MonoBehaviour
{
    public Transform RechargeArm;
    public float ChargingRate = 10;
    public BatteryPower Battery_1;
    public BatteryPower Battery_2;

    bool _charging = false;
    float _checkIfAbleToChargeTimer = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChargeAble")
        {
            _charging = true;
            if (Battery_1 == null)
            {
                Battery_1 = other.GetComponent<BatteryPower>();
            }
            else
            {
                Battery_2 = other.GetComponent<BatteryPower>();
            }
            StartCoroutine("Charge");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ChargeAble")
        {
            _charging = false;
            if (Battery_1 == other.GetComponent<BatteryPower>())
            {
                Battery_1 = null;
            }
            else
            {
                Battery_2 = null;
            }
        }
    }

    void Update()
    {
        _checkIfAbleToChargeTimer += Time.deltaTime;
        if (_checkIfAbleToChargeTimer > 1f)
        {
            _checkIfAbleToChargeTimer = 0.0f;

            if (!_charging && Battery_1 != null && Battery_2 != null)
            {
                if (Battery_2.CurrentPowerLevel < 100 && Battery_1.CurrentPowerLevel > 0)
                {
                    _charging = true;
                    StartCoroutine("Charge");
                }
            }
        }
    }

    IEnumerator Charge()
    {

            while (_charging && Battery_1.CurrentPowerLevel > 0 && Battery_2.CurrentPowerLevel < 100)
            {
                Battery_2.AdjustPowerLevel(ChargingRate * Time.deltaTime);
                Battery_1.AdjustPowerLevel(-ChargingRate * Time.deltaTime);
                yield return false;
           }

        _charging = false;
        yield return true;
    }
}
