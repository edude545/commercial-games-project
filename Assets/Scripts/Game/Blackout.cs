using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{

    public static Blackout Instance;

    bool direction; // true is fade to black, false is fade FROM black
    float time;
    float fadeTime;
    Action midFadeEvent;

    public float Speed = 1f;

    RawImage img;

    public void Start() {
        img = GetComponent<RawImage>();
        Instance = this;
        gameObject.SetActive(false);
        Debug.Log("Blackout no longer active");
    }

    public void Update() {
        time += Speed * Time.deltaTime;
        img.color = new Color(img.color.r, img.color.g, img.color.b, direction ? time : 1f - time);
        if (time >= fadeTime) {
            time = 0f;
            if (direction) {
                direction = false;
                onFadeOutFinished();
            } else {
                onFadeInFinished();
            }
        }
    }

    public static void FadeToBlack(Action _midFadeEvent, float _fadeTime) {
        Instance.midFadeEvent = _midFadeEvent;
        Instance.direction = true;
        Instance.time = 0f;
        Instance.fadeTime = _fadeTime;
        Instance.gameObject.SetActive(true);
        Player.Instance.LockControls();
    }

    void onFadeOutFinished() {
        midFadeEvent.Invoke();
    }

    void onFadeInFinished() {
        gameObject.SetActive(false);
        Player.Instance.UnlockControls();
    }

}
