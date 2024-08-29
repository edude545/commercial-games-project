using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// this script should be attached to the camera

public class HeadBob : MonoBehaviour
{
    [HideInInspector]
    public bool Bobbing = false;

    private Vector3 originalLocalPosition;
    private float osc = 0f;

    [Range(0.0f, 1.0f)]
    public float ReturnSpeed = 0.08f;
    public float ReturnSnapDistance = 0.01f;

    [Range(0.0f, 1.0f)]
    public float BobSpeed = 0.0364f;

    public float HorizontalBobDistance = 0.07f;
    public float VerticalBobDistance = 0.02f;

    private float oscDirection = 1f;

    private void Awake() {
        originalLocalPosition = transform.localPosition;
    }

    private void oscillate() {
        //float dir = (val >= 0f) ? 1f : -1f;
        float newval = osc + BobSpeed * oscDirection;
        if ((newval < -1f) || (newval > 1f)) {
            osc = oscDirection * (2f - BobSpeed) - osc;
            oscDirection *= -1f;
        } else {
            osc = newval;
        }
        /*if (val >= 0f) {
            if (val + speed > 1f) {
                val = 2f - speed - val;
            } else {
                val = val + speed;
            }
        } else {
            if (val - speed < -1f) {
                val = -2f - speed - val;
            } else {
                val = val - speed;
            }
        }
        return val;*/
    }

    private void updatePosition() {
        transform.localPosition = originalLocalPosition + transform.localRotation * new Vector3(ease(osc) * HorizontalBobDistance, Mathf.Abs(ease(osc)) * VerticalBobDistance, 0f);
    }

    private float ease(float x) {
        if (x >= 0f) {
            return x * (2 - x);
        } else {
            return x * (2 + x);
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            osc = 1f;
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            osc = -1f;
        }
        if (Bobbing) {
            oscillate();
        } else {
            osc *= 1 - ReturnSpeed;
        }
        updatePosition();
    }

}
