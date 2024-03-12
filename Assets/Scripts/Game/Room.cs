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
    }

    public void Update() {
        foreach (Anomaly anom in anomalies) {
            if (anom.isActiveAndEnabled && House.PlayerRoom != null && Distances[House.PlayerRoom] >= anom.MinimumRoomDistanceToTrigger) {
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
        }
    }

}