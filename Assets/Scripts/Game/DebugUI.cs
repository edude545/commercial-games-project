using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{

    public TMP_Text RoomLabel;

    public static DebugUI Instance;

    private void Awake() {
        Instance = this;
    }

    public static void UpdateRoomLabel(House house) {
        if (house.PlayerRoom == null) {
            Instance.RoomLabel.text = "outside";
        } else {
            int dist = house.Rooms[3].Distances[house.PlayerRoom];
            Instance.RoomLabel.text = $"{house.PlayerRoom.name}, {dist} away from {house.Rooms[3].name}";
            if (dist >= 2) {
                Instance.RoomLabel.text += "\nAnomaly can trigger here";
            }
        }
    }

}
