using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    public House House;

    public Room[] AdjacentRooms;
    public UnityEvent[] OnRoomEntered;

    public Dictionary<Room, int> Distances;
    private Anomaly[] anomalies;

    public void Awake() {
        foreach (Room room in AdjacentRooms) {
            if (!room.IsAdjacent(this)) {
                Debug.LogWarning($"Warning: {name} has a one-way adjacency to {room.name}!");
            }
        }
    }

    public void Start() {
        FindChildren();
        foreach (Anomaly anom in anomalies) {
            anom.enabled = false;
        }
    }

    public void Update() {
        foreach (Anomaly anom in anomalies) {
            if (
                anom.isActiveAndEnabled &&
                !anom.IsTriggered &&
                House.PlayerRoom != null &&
                (Distances[House.PlayerRoom] >= anom.MinimumRoomDistanceToTrigger) &&
                (!anom.HasTriggered || anom.CanTriggerMultipleTimes)
                ) {
                anom.WhileTriggerConditionsMet();
            }
        }
    }

    public void FindChildren() {
        var list = new List<Anomaly>();
        for (int i = 0; i < transform.childCount; i++) {
            Anomaly anom = transform.GetChild(i).GetComponent<Anomaly>();
            if (anom != null) {
                list.Add(anom);
            }
        }
        anomalies = list.ToArray();
    }

    public bool IsAdjacent(Room room) {
        return AdjacentRooms.Contains(room);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            House.OnPlayerEnteredRoom(this);
            DebugUI.UpdateRoomLabel(House);
            foreach (Anomaly anom in anomalies) {
                anom.enabled = true;
                Debug.Log($"Activated anomaly {anom.name}");
            }
        }
    }

}