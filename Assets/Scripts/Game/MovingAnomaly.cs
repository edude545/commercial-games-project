using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnomaly : Anomaly {

    private Vector3 initialPosition;
    public Transform destinationPosition;

    public void Awake() {
        initialPosition = transform.position;
    }

    public override void OnAnomalyTriggered() {
        transform.position = destinationPosition.position;
        Debug.Log("Object shifted!");
    }

    public override void OnAnomalyFixed() {
        transform.position = initialPosition;
    }

}
