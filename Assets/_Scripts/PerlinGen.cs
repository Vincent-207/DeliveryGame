using UnityEngine;

public class PerlinGen : MeshGen
{
    [SerializeField]
    internal
    float frequency, amplitude;
    public bool regen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    internal void Start()
    {
        CreateMesh();
        ApplyNoise();
        UpdateMesh();
    }

    internal virtual void ApplyNoise()
    {
        for(int vert = 0; vert < mesh.vertices.Length; vert++)
        {
            Vector3 vertice = mesh.vertices[vert];
            vertice.y = amplitude * Mathf.PerlinNoise(vertice.x / frequency, vertice.z / frequency);
            verticies[vert] = vertice;
        }
    }
    void Update()
    {
        if(regen)
        {
            ApplyNoise();
            UpdateMesh();
            regen = false;
        }
    }


}
