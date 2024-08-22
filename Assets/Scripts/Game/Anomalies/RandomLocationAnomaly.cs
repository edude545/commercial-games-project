using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLocationAnomaly : Anomaly
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Transform[] destinationPositions; // Array of possible destination positions
    public Transform[] destinationRotations; // Array of possible destination rotations

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
        if (destinationPositions.Length > 0 && destinationRotations.Length > 0) {
            int randomIndex = Random.Range(0, destinationPositions.Length);
            transform.position = destinationPositions[randomIndex].position;
            transform.rotation = destinationRotations[randomIndex].rotation;
            Debug.Log("Object shifted to random location!");

            // Play sound from the AudioSource attached to the same GameObject
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        else {
            Debug.LogWarning("No destination positions or rotations set!");
        }
    }

    public override void OnAnomalyFixed()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        // Stop the audio if it is set to loop
        if (audioSource != null && audioSource.isPlaying && audioSource.loop) {
            audioSource.Stop();
        }
        IsTriggered = false;
        enabled = false;
    }
}