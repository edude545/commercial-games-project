using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pianoAnomaly : Anomaly
{
    private void Start()
    {
        setSoundActive(false);
    }

    public override void OnAnomalyFixed()
    {
        setSoundActive(false);
    }

    public override void OnAnomalyTriggered()
    {
        setSoundActive(true);
    }

    private void setSoundActive(bool active)
    {
        GetComponent<AudioSource>().enabled = active;
    }
 

}
