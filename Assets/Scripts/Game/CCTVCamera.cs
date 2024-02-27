using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTVCamera : MonoBehaviour
{

    public Camera cam;
    public RenderTexture renderTexture { get; private set; }

    private void Awake() {
        renderTexture = new RenderTexture(80, 60, 16);
        cam.targetTexture = renderTexture;
        cam.Render();
        Debug.Log($"Set render texture {renderTexture}");
    }

}
