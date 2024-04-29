using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public GameObject soundSource; // Reference to the GameObject containing the AudioSource component
    public int enterCountThreshold = 3; // Number of times the player needs to enter the collider
    private AudioSource audioSource; // Reference to the AudioSource component
    private int enterCount = 0; // Counter for the number of times the player has entered
    private bool hasPlayed = false; // Flag to keep track of whether the audio has been played

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a GameObject with a Rigidbody
        if (other.attachedRigidbody != null)
        {
            // Increment the enter count
            enterCount++;

            // Check if the enter count meets the threshold and the audio hasn't played yet
            if (enterCount >= enterCountThreshold && !hasPlayed)
            {
                // Get the AudioSource component from the referenced GameObject
                audioSource = soundSource.GetComponent<AudioSource>();

                // Check if AudioSource component is not attached, then add it
                if (audioSource == null)
                {
                    Debug.LogError("AudioSource component not found on the referenced GameObject.");
                    return;
                }

                // Play the sound clip
                audioSource.Play();
                // Set the flag to true so that the sound won't play again
                hasPlayed = true;
            }
        }
    }
}