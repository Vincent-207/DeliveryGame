using UnityEngine;

[CreateAssetMenu(fileName = "PyramidBakeSettings", menuName = "Scriptable Objects/PyramidBakeSettings")]
public class PyramidBakeSettings : ScriptableObject
{
    public Mesh mesh;
    public int sourceSubMeshIndex;
    public Vector3 scale;
    public Vector3 rotation;
    public float sideLength;
}
