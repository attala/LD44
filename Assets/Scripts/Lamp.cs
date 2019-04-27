using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public Light lightSource;
    public Material MaterialOn, MaterialOff;
    private bool _on = false;

    public bool On { get => _on; set => _on = value; }

    public void Switch(bool on)
    {
        if (on)
        {
            _on = true;
            lightSource.color = Color.green;
            GetComponent<Renderer>().material = MaterialOn;
        }
        else
        {
            _on = false;
            lightSource.color = Color.red;
            GetComponent<Renderer>().material = MaterialOff;
        }
    }

}
