using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeController : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the audio source
    public float maxVolume = 1.0f; // Maximum volume
    public float minVolume = 0.0f; // Minimum volume

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the trigger collider, increase volume
            audioSource.volume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player exited the trigger collider, decrease volume
            audioSource.volume = minVolume;
        }
    }
}