using System;
using UnityEditor;
using UnityEngine;

public class SaveMeshGen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public String meshName;
    public bool doSave;

    void Update()
    {
        if(doSave)
        {
            Save();
            doSave = false;
        }        
    }

    void Save()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        AssetDatabase.CreateAsset(mesh, "Assets/Maps/" + meshName + ".asset");
        AssetDatabase.SaveAssets();
        
    }
}
