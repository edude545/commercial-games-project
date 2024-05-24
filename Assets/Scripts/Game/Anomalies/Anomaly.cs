using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Anomaly : MonoBehaviour
{

    [Tooltip("How many rooms away from the anomaly the player has to be in order for it to trigger. 0 means it can trigger while the player is in the same room, 1 means adjacent rooms, 2 means rooms adjacent to those, etc..")]
    public int MinimumRoomDistanceToTrigger = 2;

    [Tooltip("Chance each frame for this anomaly to trigger if all other conditions are met.\nChance to trigger after a minute is 1-(1-p)^3600 For reference:\n0.001 - 97%\n0.0005 - 83%\n0.0004 - 76%\n0.0003 - 66%\n0.0002 - 51%\n0.0001 - 30%\n")]
    [Range(0f, 0.01f)]
    public float TriggerChancePerFrame = 0.0003f;

    public bool CanTriggerMultipleTimes = false;

    [HideInInspector]
    public bool IsTriggered = false;
    [HideInInspector]
    public bool HasTriggered = false;

    public static int anomalyCount = 0;
    public static int activeAnomalyCount = 0;

    public virtual void WhileTriggerConditionsMet() {
        if (Random.Range(0f, 1f) < TriggerChancePerFrame)
        {
            IsTriggered = true;
            HasTriggered = true;
            activeAnomalyCount++;
            Debug.Log($"Anomaly {name} triggered");
            OnAnomalyTriggered();
        }
    }

    public virtual void OnInteract() {
        if (IsTriggered)
        {
            anomalyCount++;
            IsTriggered = false;
            activeAnomalyCount--;
            Blackout.FadeToBlack(OnAnomalyFixed);
            Debug.Log($"Anomalies solved:{activeAnomalyCount}");
        }
    }

    public abstract void OnAnomalyTriggered();

    public abstract void OnAnomalyFixed();

}
 