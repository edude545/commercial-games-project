using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnomaly : Anomaly
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Transform destinationPosition;
    public Transform destinationRotation;

    // Reference to the AudioSource component on the same GameObject
    private AudioSource audioSource;

    public void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Get AudioSource component on the same GameObject
        audioSource = GetComponent<AudioSource>();
        // Ensure that AudioSource is enabled
        if (audioSource == null)
        {
            // Add AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void OnAnomalyTriggered()
    {
        transform.position = destinationPosition.position;
        transform.rotation = destinationRotation.rotation;
        Debug.Log("Object shifted!");

        // Play sound from the AudioSource attached to the same GameObject
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public override void OnAnomalyFixed()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}