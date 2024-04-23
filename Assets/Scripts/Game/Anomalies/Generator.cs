using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Anomaly
{
    public bool generatorFixed = false;

    private void Start()
    {
        setSoundActive(true);
    }

    public override void OnAnomalyFixed()
    {
        setSoundActive(true);
        generatorFixed = true;
    }

    public override void OnAnomalyTriggered()
    {

        setSoundActive(false);
    }

    private void setSoundActive(bool active)
    {
        GetComponent<AudioSource>().enabled = active;
    }
}
