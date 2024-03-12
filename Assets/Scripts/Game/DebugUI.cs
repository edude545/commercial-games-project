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
        Instance.RoomLabel.text = house.PlayerRoom == null ? "outside" : house.PlayerRoom.name;
    }

}
