using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public Light lightSource;
    public Material MaterialOn, MaterialOff;

    public void Switch(bool on)
    {
        if (on)
        {
            lightSource.color = Color.green;
            GetComponent<Renderer>().material = MaterialOn;
        }
        else
        {
            lightSource.color = Color.red;
            GetComponent<Renderer>().material = MaterialOff;
        }
    }

}
