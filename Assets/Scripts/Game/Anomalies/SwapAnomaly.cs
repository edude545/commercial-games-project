using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAnomaly : Anomaly
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public SwapAnomalyDestination[] destinationObjects; // Array of possible destination objects

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
            initialPositions[i] = destinationObjects[i].transform.position;
            initialRotations[i] = destinationObjects[i].transform.rotation;
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

    public void Start() {
        foreach (SwapAnomalyDestination dest in destinationObjects) {
            dest.Swapper = this;
            dest.enabled = false;
        }
    }

    public override void OnAnomalyTriggered() {
        if (destinationObjects.Length > 0) {
            // Randomly select a destination object from the array
            int randomIndex = Random.Range(0, destinationObjects.Length);
            SwapAnomalyDestination destination = destinationObjects[randomIndex];
            destination.enabled = true;

            // Store the index of the last swapped object
            lastSwappedIndex = randomIndex;

            // Store current position and rotation of the destination object
            Vector3 destinationPosition = destination.transform.position;
            Quaternion destinationRotation = destination.transform.rotation;

            // Swap positions and rotations
            destination.transform.position = transform.position;
            destination.transform.rotation = transform.rotation;
            transform.position = destinationPosition;
            transform.rotation = destinationRotation;

            Debug.Log("Objects swapped!");

            // Play sound from the AudioSource attached to the same GameObject
            if (audioSource != null && audioSource.clip != null) {
                audioSource.Play();
            }
        } else {
            Debug.LogWarning("No destination objects set!");
        }
    }

    public override void OnAnomalyFixed() {
        // Restore initial position and rotation of the anomaly object
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Restore initial position and rotation of the last swapped object if any
        if (lastSwappedIndex != -1) {
            destinationObjects[lastSwappedIndex].transform.position = initialPositions[lastSwappedIndex];
            destinationObjects[lastSwappedIndex].transform.rotation = initialRotations[lastSwappedIndex];
        }

        // Stop the audio if it is set to loop
        if (audioSource != null && audioSource.isPlaying && audioSource.loop) {
            audioSource.Stop();
        }

        destinationObjects[lastSwappedIndex].GetComponent<SwapAnomalyDestination>().enabled = false;

        // Reset the last swapped index
        lastSwappedIndex = -1;
    }
}
