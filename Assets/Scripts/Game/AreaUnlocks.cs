using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaUnlocks : MonoBehaviour
{
    private GameObject[] objectsWithTag;
    public int requiredAnomalyCount = 5; // Specify the required anomaly count
    public TextMeshProUGUI messageText; // Reference to the UI Text element
    public float messageDuration = 2f; // Duration for which the message will be displayed

    private bool displayMessage = false;
    private float messageTimer = 0f;

    void Start()
    {
        objectsWithTag = GameObject.FindGameObjectsWithTag("block");
        UpdateAreaUnlockStatus();
        messageText.text = "Anomalies solved: " + Anomaly.anomalyCount;
    }

    void Update()
    {
        UpdateAreaUnlockStatus(); // Continuously update area unlock status

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
            SetObjectsActive(false);
        }
        else
        {

            SetObjectsActive(true);
        }
    }

    void SetObjectsActive(bool active)
    {
        foreach (GameObject obj in objectsWithTag)
        {
            obj.SetActive(active);
        }
    }

 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("block"))
        {
            // Display a message when player collides with the collider
            messageText.text = "Solve 10 anomalies, anomalies currently solved: " + Anomaly.anomalyCount;
            displayMessage = true;
        }
    }


}