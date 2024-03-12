using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Anomaly : MonoBehaviour
{

    [Tooltip("How many rooms away from the anomaly the player has to be in order for it to trigger. 0 means it can trigger while the player is in the same room, 1 means adjacent rooms, 2 means rooms adjacent to those, etc..")]
    public int MinimumRoomDistanceToTrigger = 2;

    [Tooltip("Chance each frame for this anomaly to trigger if all other conditions are met.")]
    [Range(0f, 1f)]
    public float TriggerChancePerFrame = 0.01f;

    public bool IsTriggered = false;

    public virtual void WhileTriggerConditionsMet() {
        if (Random.Range(0f,1f) < TriggerChancePerFrame) {
            OnAnomalyTriggered();
        }
    }

    public abstract void OnAnomalyTriggered();

}
 