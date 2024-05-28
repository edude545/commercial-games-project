using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Anomaly
{
    public static Generator Instance;

    public bool generatorFixed = false;
    public GameObject endGameObject; // Reference to the EndGame GameObject
    public Mannequin NormalMannequin;
    public ChaseMannequin ChaseMannequin;
    public AudioSource MannequinActivationSound;

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        SetSoundActive(true);
        SetAllLights(false);
        NormalMannequin.gameObject.SetActive(true);
        ChaseMannequin.gameObject.SetActive(false);
    }

    public override void OnAnomalyFixed()
    {
        SetSoundActive(true);
        generatorFixed = true;
        // Enable the EndGame GameObject
        SetAllLights(true);
        endGameObject.SetActive(true);
        NormalMannequin.gameObject.SetActive(false);
        ChaseMannequin.gameObject.SetActive(true);
        MannequinActivationSound.Play();
        ChaseMannequin.StartChaseSequence();
    }

    public override void OnAnomalyTriggered()
    {
        SetSoundActive(false);
        SetAllLights(false);
        // Disable the box collider
    }

    private void SetSoundActive(bool active)
    {
        GetComponent<AudioSource>().enabled = active;
    }

    public void SetAllLights(bool active) {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Light")) {
            obj.GetComponent<Light>().enabled = active;
        }
    }
}