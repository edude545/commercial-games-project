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

    public Room PlayerRoom { get; private set; }

    private Room[] rooms;

    public void Start() {
        InitializeHouse();
    }

    private void initializeRooms() {
        List<Room> roomsList = new List<Room>();
        for (int i = 0; i < RoomsParent.childCount; i++) {
            Transform floor = RoomsParent.GetChild(i).transform;
            for (int j = 0; j < floor.childCount; j++) {
                Room room = floor.GetChild(j).GetComponent<Room>();
                roomsList.Add(room);
                room.House = this;
            }
        }
        rooms = roomsList.ToArray();
    }

    private void printlist(IEnumerable list) {
        string s = "";
        foreach (var el in list) {
            s += el.ToString() + ", ";
        }
        Debug.Log(s.Substring(0, s.Length - 2));
    }

    private void roomDijkstra(Room source) {
        var dist = new Dictionary<Room, int>();
        var prev = new Dictionary<Room, Room>();
        var q = new List<Room>();
        foreach (Room room in rooms) {
            dist[room] = int.MaxValue;
            q.Add(room);
        }
        dist[source] = 0;
        while (q.Count > 0) {
            Room closest = source;
            // min = vertex in q with min dist[u]
            int min = int.MaxValue;
            foreach (Room room in q) {
                Debug.Log($"Distance to {room.name} is {dist[room]}");
                if (dist[room] < min && room != source) {
                    Debug.Log($"{room.name}'s distance {dist[room]} lower than {min}; choosing this as new closest room");
                    closest = room;
                    min = dist[room];
                }
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
            s += $"{key.name} : {dist[key]}\n";
        }
        source.Distances = dist;
    }

    public void InitializeHouse() {
        initializeRooms();
        foreach (Room room in rooms) {
            roomDijkstra(room);
        }
    }

    public void OnPlayerEnteredRoom(Room room) {
        PlayerRoom = room;
    }

}

[CustomEditor(typeof(House))]
public class HouseEditor : UnityEditor.Editor {

    private House house;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Initialize")) {
            house.InitializeHouse();
        }
    }

    private void OnEnable() {
        house = (House)target;
    }

}