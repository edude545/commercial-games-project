using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{

    public static Blackout Instance;

    public float FadeInterval = 0.5f;
    public float CharacterInterval = 0.2f;

    enum State {
        FADE_OUT, // Fade to black
        HOLD, // Hold and display text
        FADE_IN // Fade from black
    }

    State state; // true is fade to black, false is fade FROM black
    float time;
    float interval;
    float fadeInterval;
    Action midFadeEvent;
    public TMP_Text Label;

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
        if (state == State.FADE_OUT) {
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(0f, 1f, time));
            Label.color = img.color;
            if (time >= interval) { // Fade out finished
                time = 0f;
                state = State.HOLD;
                interval = CharacterInterval;
                midFadeEvent.Invoke();
            }
        } else if (state == State.HOLD) {
            if (time >= interval) {
                time -= interval;
                Label.maxVisibleCharacters += 1;
            }
            if (Label.maxVisibleCharacters == Label.text.Length) { // Hold finished
                time = 0f;
                state = State.FADE_IN;
                interval = FadeInterval;
            }
        } else if (state == State.FADE_IN) {
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(1f, 0f, time));
            Label.color = img.color;
            if (time >= interval) { // Fade in finished
                time = 0f;
                state = State.HOLD;
                gameObject.SetActive(false);
                Player.Instance.UnlockControls();
            }
        }
    }

    public static void FadeToBlack(Action _midFadeEvent) {
        Instance.midFadeEvent = _midFadeEvent;
        Instance.state = State.FADE_OUT;
        Instance.time = 0f;
        Instance.interval = Instance.FadeInterval;
        Instance.Label.maxVisibleCharacters = 0;
        Instance.gameObject.SetActive(true);
        Player.Instance.LockControls();
    }

}
