using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTVScreen : MonoBehaviour
{

    public CCTVCamera Camera;

    private void Start() {
        loadRenderTexture();
    }

    private void OnValidate() {
        loadRenderTexture();
    }

    private void loadRenderTexture() {
        if (Camera == null) return;
        //Debug.Log("Screen getting render texture...");
        Debug.Log("Loading render texture");
        RawImage ri = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        ri.texture = Camera.renderTexture;
        //ri.SetNativeSize();
        //Debug.Log($"{ri.texture.width}, {ri.texture.height}");
        //((RectTransform)ri.transform).sizeDelta /= 32f;
    }

}
