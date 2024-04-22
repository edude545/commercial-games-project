using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnomaly : Anomaly
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Transform destinationPosition;
    public Transform destinationRotation;

    public void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public override void OnAnomalyTriggered()
    {
        transform.position = destinationPosition.position;
        transform.rotation = destinationRotation.rotation;
        Debug.Log("Object shifted!");
    }

    public override void OnAnomalyFixed()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}