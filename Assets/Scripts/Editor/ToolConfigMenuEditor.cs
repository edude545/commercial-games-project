using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ToolConfigMenu))]
public class ToolConfigMenuEditor : Editor
{

    ToolConfigMenu tcm;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Debug generate UI")) {
            tcm.DebugGenerateUI();
        }
    }

    private void OnEnable() {
        tcm = (ToolConfigMenu)target;
    }

}
