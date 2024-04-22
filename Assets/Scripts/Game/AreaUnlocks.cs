using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaUnlocks : MonoBehaviour
{
    public int requiredAnomalyCount = 5; // Specify the required anomaly count for unlocking
    public TextMeshProUGUI messageText; // Reference to the UI Text element
    public float messageDuration = 2f; // Duration for which the message will be displayed

    private bool displayMessage = false;
    private float messageTimer = 0f;

    void Start()
    {
        UpdateAreaUnlockStatus();
        messageText.text = "Anomalies solved: " + Anomaly.anomalyCount;
    }

    void Update()
    {
        if (displayMessage)
        {
            messageTimer += Time.deltaTime;
            if (messageTimer >= messageDuration)
            {
                // Hide the message after the duration
                messageText.text = "";
                displayMessage = false;
                messageTimer = 0f;
            }
        }
    }

    void UpdateAreaUnlockStatus()
    {
        if (Anomaly.anomalyCount >= requiredAnomalyCount)
        {
            Destroy(gameObject); // Destroy this GameObject if the required anomaly count is met
            displayMessage = true; // Optionally display a message
        }
    }

    public void SetRequiredAnomalyCount(int count)
    {
        requiredAnomalyCount = count;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Update the area unlock status when the player collides with the collider
            UpdateAreaUnlockStatus();

            // Display a message when the player collides with the collider
            messageText.text = "Solve " + requiredAnomalyCount + " anomalies, anomalies currently solved: " + Anomaly.anomalyCount;
            displayMessage = true;
        }
    }

    void OnDestroy()
    {
        // Clear the message text when the GameObject is destroyed
        messageText.text = "";
    }
}