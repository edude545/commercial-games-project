using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUnlocks : MonoBehaviour
{
    private GameObject[] objectsWithTag;
    public int requiredAnomalyCount = 5; // Specify the required anomaly count

    void Start()
    {
        objectsWithTag = GameObject.FindGameObjectsWithTag("block");
        UpdateAreaUnlockStatus();
    }

    void Update()
    {
        UpdateAreaUnlockStatus(); // Continuously update area unlock status
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
}