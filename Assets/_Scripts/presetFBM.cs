using UnityEngine;
using UnityEngine.Rendering;

public class presetFBM : MonoBehaviour
{
    Mesh mesh;
    Vector3[] verticies;
    public int octaves;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        verticies = mesh.vertices;

    }

    void Generate()
    {
        
    }
}
