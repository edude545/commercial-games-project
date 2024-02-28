using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CCTVCamera : MonoBehaviour
{

    public Vector2Int Resolution;

    public Camera cam;
    public RenderTexture renderTexture { get; private set; }

    private void Awake() {
        MakeRenderTexture();
    }

    public void MakeRenderTexture() {
        renderTexture = new RenderTexture(Resolution.x, Resolution.y, 0);
        renderTexture.filterMode = FilterMode.Point;
        cam.targetTexture = renderTexture;
        Debug.Log($"Generated render texture {renderTexture}");
    }

}

[CustomEditor(typeof(CCTVCamera))]
public class CCTVCameraEditor : UnityEditor.Editor {

    private CCTVCamera cctvCamera;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Make render texture")) {
            cctvCamera.MakeRenderTexture();
        }
    }

    private void OnEnable() {
        cctvCamera = (CCTVCamera) target;
    }

}