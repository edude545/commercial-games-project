using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaseMannequinDebugMeter : MonoBehaviour
{

    public TMP_Text tmp;

    public ChaseMannequin mannequin;

    void Start()
    {
        tmp = GetComponent<TMP_Text>();
    }

    void Update()
    {
        tmp.text = $"Chasing: {mannequin.Chasing}\nPosing: {mannequin.Posing}\nDistance: {(mannequin.transform.position-Player.Instance.transform.position).magnitude}";
    }
}
