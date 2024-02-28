using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;

/*
 WORK IN PROGRESS
    - lyra
*/

[System.Serializable, CreateAssetMenu(fileName = "PSXMaterialGenerator", menuName = "Lyra/PSXMaterialGenerator")]
public class PSXMaterialGenerator : ScriptableObject {

    public string MeshesPath;

}


[CustomEditor(typeof(PSXMaterialGenerator))]
public class PSXMaterialGeneratorEditor : UnityEditor.Editor {

    private PSXMaterialGenerator generator;
    private Shader psxShader;
    private string path;
    private string savepath;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate materials")) {
            try {
                generateMaterials();
            } catch (Exception e) {
                AssetDatabase.StopAssetEditing();
                Debug.LogError(e);
            }
        }
    }

    private void OnEnable() {
        generator = (PSXMaterialGenerator)target;
    }

    private void generateMaterials() {
        AssetDatabase.StartAssetEditing();
        psxShader = Shader.Find("Shader Graphs/URP_PSX_Unlit_Master");
        string[] patharray = AssetDatabase.GetAssetPath(generator.GetInstanceID()).Split("/");
        patharray = patharray.Take(patharray.Length - 1).ToArray();
        path = String.Join("/", patharray);
        generateMaterials($"{path}/{generator.MeshesPath}");
        savepath = path + "/PSX Materials";
        Debug.Log($"savepath: {savepath}");
        if (!AssetDatabase.IsValidFolder(savepath)) {
            AssetDatabase.CreateFolder(path, "PSX Materials");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.StopAssetEditing();
    }

    private void generateMaterials(string searchpath) {
        foreach (string subpath in AssetDatabase.GetSubFolders(searchpath)) {
            //Debug.Log($"{path}, {subpath}");
            //Debug.Log(subpath.Replace(path, path + "/PSX Materials"));
            generateMaterials(subpath);
        }
        //string psxmatpath = searchpath.Replace(path, path + "/PSX Materials");
        /*if (!AssetDatabase.IsValidFolder(psxmatpath)) {
            AssetDatabase.CreateFolder(path, "PSX Materials");
        }*/
        //Debug.Log($"searchpath: {searchpath}\npsxmatpath: {psxmatpath}");
        Debug.Log($"searchpath: {searchpath}");
        foreach (string assetpath in Directory.GetFiles(searchpath, "*.fbx", SearchOption.TopDirectoryOnly)) {
            foreach (UnityEngine.Object asset in AssetDatabase.LoadAllAssetsAtPath(assetpath)) {
                Material mat = asset as Material;
                if (mat != null) {
                    Debug.Log($"{mat.name}, {mat.mainTexture.name}");
                    Material psxmat = new Material(psxShader);
                    psxmat.mainTexture = mat.mainTexture;
                    //AssetDatabase.CreateAsset(psxmat, psxmatpath+"/"+mat.name+"_psx.asset");
                    AssetDatabase.CreateAsset(psxmat, $"{savepath}/{mat.name}_psx.asset");
                }
                //Debug.Log($"{ass.name}, {true?"material":"not material"}");
            }
            //ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(assetpath);
            //Debug.Log(importer.materials);
        }
    }

}