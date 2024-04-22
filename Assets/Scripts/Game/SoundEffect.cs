using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip soundClip; // Sound clip to play
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if AudioSource component is not attached, then add it
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the sound clip to the AudioSource
        audioSource.clip = soundClip;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a GameObject with a Rigidbody
        if (other.attachedRigidbody != null)
        {
            // Play the sound clip
            audioSource.Play();
        }
    }

}
