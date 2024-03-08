using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class House : MonoBehaviour
{

    public Transform RoomsParent;

    private Room[] rooms;

    public void Awake() {
        InitializeHouse();
    }

    private void initializeRooms() {
        List<Room> roomsList = new List<Room>();
        for (int i = 0; i < RoomsParent.childCount; i++) {
            roomsList.Add(RoomsParent.GetChild(i).GetComponent<Room>());
        }
        rooms = roomsList.ToArray();
    }

    private void roomDijkstra(Room source) {
        var dist = new Dictionary<Room, int>();
        var prev = new Dictionary<Room, Room>();
        var q = new List<Room>();
        foreach (Room room in rooms) {
            if (room == source) {
                dist[room] = 0; continue;
            }
            dist[room] = int.MaxValue;
            q.Add(room);
        }
        while (q.Count > 0) {
            Room closest = source;
            int min = int.MaxValue;
            foreach (Room room in q) {
                if (dist[room] < min) {
                    closest = room;
                    min = dist[room];
                }
            }
            if (closest == source) {
                Debug.LogError("Something went wrong! A room is isolated?");
            }
            q.Remove(closest);
            foreach (Room neighbor in closest.AdjacentRooms) {
                if (q.Contains(neighbor)) {
                    int alt = dist[closest] + 1;
                    if (alt < dist[neighbor]) {
                        dist[neighbor] = alt;
                        prev[neighbor] = closest;
                    }
                }
            }
        }
        string s = $"{source.name}:\n";
        foreach (Room key in dist.Keys) {
            s += $"{key.name} : {dist[key]}";
        }
        Debug.Log(s);
    }

    public void InitializeHouse() {
        initializeRooms();
        foreach (Room room in rooms) {
            roomDijkstra(room);
        }
    }

}

[CustomEditor(typeof(House))]
public class HouseEditor : UnityEditor.Editor {

    private House house;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Debug initialize")) {
            house.InitializeHouse();
        }
    }

    private void OnEnable() {
        house = (House)target;
    }

}