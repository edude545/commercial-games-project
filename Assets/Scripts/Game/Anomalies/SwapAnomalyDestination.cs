using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAnomalyDestination : MonoBehaviour, IAnomalyFixerTarget {

    [HideInInspector]
    public SwapAnomaly Swapper;

    // This stub method needs to be here, otherwise the enabled/disabled checkbox doesn't show up in the editor
    public void Start() {
        
    }

    public bool CanBeInteractedWith() {
        return enabled;
    }

    public void OnInteract() {
        Swapper.OnInteract();
    }

}
