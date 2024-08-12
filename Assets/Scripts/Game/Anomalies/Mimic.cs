using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Anomaly
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Transform[] destinationPositions; // Array of possible destination positions
    public Transform[] destinationRotations; // Array of possible destination rotations

    private int currentPositionIndex = 0; // Track the current position index
    private bool isAnomalyActive = false; // Track if anomaly is active

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
        if (destinationPositions.Length > 0 && destinationRotations.Length > 0)
        {
            isAnomalyActive = true;
            StartCoroutine(SwitchPositions());
        }
        else
        {
            Debug.LogWarning("No destination positions or rotations set!");
        }
    }

    public override void OnAnomalyFixed()
    {
        isAnomalyActive = false;
        StopAllCoroutines();
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Stop the audio if it is set to loop
        if (audioSource != null && audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Stop();
        }

        // Reset index to 0
        currentPositionIndex = 0;
    }

    private IEnumerator SwitchPositions()
    {
        while (isAnomalyActive)
        {
            // Move to the current position in the array
            transform.position = destinationPositions[currentPositionIndex].position;
            transform.rotation = destinationRotations[currentPositionIndex].rotation;
            Debug.Log("Object shifted to location: " + currentPositionIndex);

            // Play sound from the AudioSource attached to the same GameObject
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            // Increment index and loop back if it exceeds the array length
            currentPositionIndex = (currentPositionIndex + 1) % destinationPositions.Length;

            // Wait for a few seconds before switching to the next position
            yield return new WaitForSeconds(3.0f); // Adjust the time as needed
        }
    }
}