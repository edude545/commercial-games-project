using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    public Room[] AdjacentRooms;
    public UnityEvent[] OnRoomEntered;

    public Dictionary<Room, int> distances;

    public void Awake() {
        foreach (Room room in AdjacentRooms) {
            if (!room.IsAdjacent(this)) {
                Debug.LogWarning($"Warning: {name} has a one-way adjacency to {room.name}!");
            }
        }
    }

    public bool IsAdjacent(Room room) {
        return AdjacentRooms.Contains(room);
    }



}