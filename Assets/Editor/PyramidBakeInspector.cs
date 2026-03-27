using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PyramidBakeSettings))]
public class PyramidBakeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Create"))
        {
            Debug.Log("Button presed");

        }

        
    }
}
