using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recharger : MonoBehaviour
{
    public Transform RechargeArm;
    public float ChargingRate = 10;
    public BatteryPower Battery_1;
    public BatteryPower Battery_2;
    public GameObject SparksAndSound;
    public bool Battery2IsDraining = true;

    AudioSource _chargingSound;
    AudioClip _originalChargeSound;
    public AudioClip FinishedChargeSound, FinishedChargeNot100PercentSound;
    bool _charging = false;
    float _checkIfAbleToChargeTimer = 0.0f;

    private void Start()
    {
        SparksAndSound.SetActive(false);
        _chargingSound = GetComponent<AudioSource>();
        _originalChargeSound = _chargingSound.clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChargeAble" || other.tag == "Player")
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
        if (other.tag == "ChargeAble" || other.tag == "Player")
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
                if (Battery_2.CurrentPowerLevel < 100 && Battery_1.CurrentPowerLevel > 1)
                {
                    _charging = true;
                    StartCoroutine("Charge");
                }
            }
        }
        if (_charging)
        {
            if (Battery2IsDraining)
            {
                Battery_2.transform.position = Vector3.Lerp(Battery_2.transform.position, transform.position, 0.5f * Time.deltaTime);
            }
            else
            {
                Battery_1.transform.position = Vector3.Lerp(Battery_1.transform.position, transform.position, 0.5f * Time.deltaTime);
            }
            
        }
    }

    IEnumerator Charge()
    {
        SparksAndSound.SetActive(true);
        _chargingSound.clip = _originalChargeSound;
        _chargingSound.loop = true;
        _chargingSound.Play();

            while (_charging && Battery_1.CurrentPowerLevel > 1 && Battery_2.CurrentPowerLevel < 100)
            {
                _chargingSound.pitch = (Battery_2.CurrentPowerLevel / 100) + 0.3f;
                Battery_2.AdjustPowerLevel(ChargingRate * Time.deltaTime);
                Battery_1.AdjustPowerLevel(-ChargingRate * Time.deltaTime);
                yield return false;
            }
        _chargingSound.Stop();
        _chargingSound.loop = false;
        if(Battery_2.CurrentPowerLevel > 95)
        {
            _chargingSound.clip = FinishedChargeSound;
        }
        else
        {
            _chargingSound.clip = FinishedChargeNot100PercentSound;
        }
        _chargingSound.Play();
        _charging = false;
        SparksAndSound.SetActive(false);
        yield return true;
    }
}
