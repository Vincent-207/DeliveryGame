using UnityEngine;

public class PerlinGen : MeshGen
{
    [SerializeField]
    float frequency, amplitude;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateMesh();
        ApplyPerlin();
        UpdateMesh();
    }

    void ApplyPerlin()
    {
        for(int vert = 0; vert < mesh.vertices.Length; vert++)
        {
            Vector3 vertice = mesh.vertices[vert];
            vertice.y = amplitude * Mathf.PerlinNoise(vertice.x / frequency, vertice.z / frequency);
            verticies[vert] = vertice;
        }
    }


}
