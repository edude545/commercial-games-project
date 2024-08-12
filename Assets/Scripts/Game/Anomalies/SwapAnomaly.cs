using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAnomaly : Anomaly
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Transform[] destinationObjects; // Array of possible destination objects

    private Vector3[] initialPositions; // Store initial positions of destination objects
    private Quaternion[] initialRotations; // Store initial rotations of destination objects
    private int lastSwappedIndex = -1; // Track the last swapped object index

    // Reference to the AudioSource component on the same GameObject
    private AudioSource audioSource;

    public void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Initialize arrays to store initial positions and rotations
        initialPositions = new Vector3[destinationObjects.Length];
        initialRotations = new Quaternion[destinationObjects.Length];

        // Store initial positions and rotations of destination objects
        for (int i = 0; i < destinationObjects.Length; i++)
        {
            initialPositions[i] = destinationObjects[i].position;
            initialRotations[i] = destinationObjects[i].rotation;
        }

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
        if (destinationObjects.Length > 0)
        {
            // Randomly select a destination object from the array
            int randomIndex = Random.Range(0, destinationObjects.Length);
            Transform destinationObject = destinationObjects[randomIndex];

            // Store the index of the last swapped object
            lastSwappedIndex = randomIndex;

            // Store current position and rotation of the destination object
            Vector3 destinationPosition = destinationObject.position;
            Quaternion destinationRotation = destinationObject.rotation;

            // Swap positions and rotations
            destinationObject.position = transform.position;
            destinationObject.rotation = transform.rotation;
            transform.position = destinationPosition;
            transform.rotation = destinationRotation;

            Debug.Log("Objects swapped!");

            // Play sound from the AudioSource attached to the same GameObject
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("No destination objects set!");
        }
    }

    public override void OnAnomalyFixed()
    {
        // Restore initial position and rotation of the anomaly object
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Restore initial position and rotation of the last swapped object if any
        if (lastSwappedIndex != -1)
        {
            destinationObjects[lastSwappedIndex].position = initialPositions[lastSwappedIndex];
            destinationObjects[lastSwappedIndex].rotation = initialRotations[lastSwappedIndex];
        }

        // Stop the audio if it is set to loop
        if (audioSource != null && audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Stop();
        }

        // Reset the last swapped index
        lastSwappedIndex = -1;
    }
}
