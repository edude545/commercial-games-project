using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAnomaly : Anomaly {

    public float GrowthRate = 0.001f;
    public float MaxSize;
    public bool ExpandX = true;
    public bool ExpandY = true;
    public bool ExpandZ = true;

    private Vector3 initialScale;
    private Material mat;

    public bool IsExpanding { get; private set; }
    public bool HasReachedMaxSize { get; private set; }

    private AudioSource audioSource;

    private void Start() {
        initialScale = transform.localScale;
        mat = GetComponent<MeshRenderer>().material;
        GetComponent<MeshRenderer>().enabled = false;
        IsExpanding = false;
        HasReachedMaxSize = false;

        // Get AudioSource component on the same GameObject
        audioSource = GetComponent<AudioSource>();
        // Ensure that AudioSource is enabled
        if (audioSource == null)
        {
            // Add AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update() {
        if (IsExpanding) {
            transform.localScale = new Vector3(
                ExpandX ? (transform.localScale.x + initialScale.x * GrowthRate) : transform.localScale.x,
                ExpandY ? (transform.localScale.y + initialScale.y * GrowthRate) : transform.localScale.y,
                ExpandZ ? (transform.localScale.z + initialScale.z * GrowthRate) : transform.localScale.z
                );
            if (transform.localScale.x >= MaxSize || transform.localScale.y >= MaxSize || transform.localScale.z >= MaxSize) {
                IsExpanding = false;
                HasReachedMaxSize = true;
            }
        }
    }

    public override void OnAnomalyFixed() {
        transform.localScale = initialScale;
        GetComponent<MeshRenderer>().enabled = false;
        IsExpanding = false;
        HasReachedMaxSize = false;
        if (audioSource != null && audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Stop();
        }
    }

    public override void OnAnomalyTriggered() {
        GetComponent<MeshRenderer>().enabled = true;
        IsExpanding = true;

        // Play sound from the AudioSource attached to the same GameObject
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

}
