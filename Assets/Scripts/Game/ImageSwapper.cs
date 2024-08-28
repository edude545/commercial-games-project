using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwapper : MonoBehaviour
{
    public Image image; // Reference to the Image component in the UI
    public Sprite[] images; // Array of sprites to swap between
    public AudioClip swapSound; // Reference to the audio clip to play on arrow key press

    private int currentIndex = 0; // Index of the current image
    private bool isImageVisible = false; // Flag to track if the image is currently visible
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check for Tab key press to toggle the image visibility
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Toggle the visibility of the image
            isImageVisible = !isImageVisible;
            image.enabled = isImageVisible;

            // If the image is now visible, display the first image
            if (isImageVisible)
            {
                image.sprite = images[currentIndex];
                image.SetNativeSize();
            }
        }

        // If the image is visible, allow image swapping with arrow keys
        if (isImageVisible)
        {
            // Check for left arrow key press to go to the previous image
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Decrease the index
                currentIndex--;
                // Wrap around to the last image if currentIndex becomes negative
                if (currentIndex < 0)
                {
                    currentIndex = images.Length - 1;
                }
                // Update the displayed image
                image.sprite = images[currentIndex];
                image.SetNativeSize();

                // Play the swap sound
                PlaySwapSound();
            }

            // Check for right arrow key press to go to the next image
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Increase the index
                currentIndex++;
                // Wrap around to the first image if currentIndex exceeds the array length
                if (currentIndex >= images.Length)
                {
                    currentIndex = 0;
                }
                // Update the displayed image
                image.sprite = images[currentIndex];
                image.SetNativeSize();

                // Play the swap sound
                PlaySwapSound();
            }
        }
    }

    void PlaySwapSound()
    {
        // Check if the AudioSource and swapSound are assigned
        if (audioSource != null && swapSound != null)
        {
            audioSource.PlayOneShot(swapSound);
        }
    }
}