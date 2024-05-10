using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Anomaly
{
    public bool generatorFixed = false;
    public GameObject endGameObject; // Reference to the EndGame GameObject

    private void Start()
    {
        SetSoundActive(true);
    }

    public override void OnAnomalyFixed()
    {
        SetSoundActive(true);
        generatorFixed = true;
        // Enable the EndGame GameObject
        endGameObject.SetActive(true);
    }

    public override void OnAnomalyTriggered()
    {
        SetSoundActive(false);
        // Disable the box collider
    }

    private void SetSoundActive(bool active)
    {
        GetComponent<AudioSource>().enabled = active;
    }
}