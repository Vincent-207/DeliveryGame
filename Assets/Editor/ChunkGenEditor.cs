using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChunkGen))]
public class ChunkGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChunkGen chunkGen = (ChunkGen)target;
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate"))
        {
            chunkGen.Generate();
        }

    }
}
