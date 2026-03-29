using System;
using UnityEditor;
using UnityEngine;

public class SaveChunkGen : MonoBehaviour
{
    public String path = "Assets/Maps/", meshName;
    public bool DoSave;
    void Save()
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            Mesh mesh = transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh;
            AssetDatabase.CreateAsset(mesh, path + meshName + " " + i + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DoSave)
        {
            Save();
            DoSave = false;
        }
    }
}
