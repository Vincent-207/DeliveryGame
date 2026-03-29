using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class FractionalBrownianMotion : MeshGen, IChunk
{
    [SerializeField]
    public int octaves;
    public float baseFrequency;
    public float baseAmplitude;
    public bool regen;
    public int seed;
    public float scale;
    public float lacunarity = 2f, gain = 0.5f; 
    public Vector2 offset;
    public void Init(Vector2 gridPos, int seed)
    {
        offset = gridPos;
        this.seed = seed;
    }
    public void Generate()
    {
        // float startTime = Time.realtimeSinceStartup;
        // Debug.Log("Start time: " + Time.realtimeSinceStartup);
        // CreateMesh();
        // float meshCreationTime = Time.realtimeSinceStartup - startTime;
        // Debug.Log("Mesh creation time: " + meshCreationTime);
        
        // ApplyNoise();
        // float noiseTime = Time.realtimeSinceStartup - meshCreationTime;
        // Debug.Log("Noise application time: " + noiseTime);
        // UpdateMesh();
        // float meshUpdateTime = Time.realtimeSinceStartup - noiseTime;
        // Debug.Log("Mesh update time: " + meshUpdateTime);
        CreateMesh();
        ApplyNoise();
        UpdateMesh();
        UpdateMesh();
    }
    void ApplyNoise()
    {
        for(int vert = 0; vert < mesh.vertices.Length; vert++)
        {
            Vector3 vertice = new(mesh.vertices[vert].x , 0,  mesh.vertices[vert].z);
            float noise = GetNoiseAtPos(new(vertice.x + offset.x, vertice.z + offset.y));
            // Debug.Log("NOISE: " + noise);
            vertice.y = noise;
            verticies[vert] = vertice;
        }
    }
    IEnumerator PrintDeltaTime()
    {
        Debug.Log("Delta time: " + Time.deltaTime);
        yield return null;
        Debug.Log("Delta time: " + Time.deltaTime);
    }
    float GetNoiseAtPos(Vector2 pos)
    {
        return GetNoiseAtPos(pos.x, pos.y);
    }
    float GetNoiseAtPos(float x, float z)
    {
        float amplitude = baseAmplitude;
        float frequency = baseFrequency;

        float sum  = 0;

        for(int i = 0; i < octaves; i++)
        {
            float sampleX = x / scale * frequency;
            float sampleZ = z / scale * frequency;
            sum += Mathf.PerlinNoise(sampleX, sampleZ) * amplitude;
            // Debug.Log("Elevation: " + elevation);
            frequency *= lacunarity;
            amplitude *= gain;
        }
        // elevation = Mathf.Clamp(elevation, minHeight, maxHeight);
        return sum;
    }

    void Update()
    {
        if(regen)
        {
            // CreateMesh();
            // ApplyNoise();
            // UpdateMesh();
            Generate();
            regen = false;
        }
    }
}

public interface IChunk
{
    public void Init(Vector2 gridPos, int seed);
    public void Generate();
}