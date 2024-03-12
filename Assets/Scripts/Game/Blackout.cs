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

    enum State {
        FADE_OUT, // Fade to black
        HOLD, // Hold and display text
        FADE_IN // Fade from black
    }

    State state; // true is fade to black, false is fade FROM black
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
        if (state == State.FADE_IN || state == State.FADE_OUT) {
            time += Speed * Time.deltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, state == State.FADE_OUT ? time : 1f - time);
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
    }

    public static void FadeToBlack(Action _midFadeEvent, float _fadeTime) {
        Instance.midFadeEvent = _midFadeEvent;
        Instance.state = State.FADE_OUT;
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
