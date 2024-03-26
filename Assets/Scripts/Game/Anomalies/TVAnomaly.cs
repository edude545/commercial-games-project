using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVAnomaly : Anomaly {

    public Material NormalMaterial;
    public Material TriggeredMaterial;
    public MeshRenderer Renderer;

    private void Start() {
        setSoundActive(false);
    }

    public override void OnAnomalyFixed() {
        setScreenMaterial(NormalMaterial);
        setSoundActive(false);
    }

    public override void OnAnomalyTriggered() {
        setScreenMaterial(TriggeredMaterial);
        setSoundActive(true);
    }

    private void setSoundActive(bool active) {
        GetComponent<AudioSource>().enabled = active;
    }

    private void setScreenMaterial(Material mat) {
        Material[] mats = Renderer.materials;
        mats[1] = mat;
        Renderer.materials = mats;
    }

}
