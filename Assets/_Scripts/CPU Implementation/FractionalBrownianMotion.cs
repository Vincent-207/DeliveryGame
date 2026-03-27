using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class FractionalBrownianMotion : MeshGen
{
    [SerializeField]
    public int octaves;
    public float baseFrequency;
    public float baseAmplitude;
    public bool regen;
    public int seed;
    public float minHeight, maxHeight;
    public float scale;
    public float lacunarity = 2f, gain = 0.5f; 

    void Start()
    {
        float startTime = Time.realtimeSinceStartup;
        Debug.Log("Start time: " + Time.realtimeSinceStartup);
        CreateMesh();
        float meshCreationTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Mesh creation time: " + meshCreationTime);
        seed = Random.Range(1, 1000000);
        ApplyNoise();
        float noiseTime = Time.realtimeSinceStartup - meshCreationTime;
        Debug.Log("Noise application time: " + noiseTime);
        UpdateMesh();
        float meshUpdateTime = Time.realtimeSinceStartup - noiseTime;
        Debug.Log("Mesh update time: " + meshUpdateTime);
    }
    void ApplyNoise()
    {
        for(int vert = 0; vert < mesh.vertices.Length; vert++)
        {
            Vector3 vertice = mesh.vertices[vert];
            vertice.y = GetNoiseAtPos(vertice.x, vertice.z);
            verticies[vert] = vertice;
        }
    }
    IEnumerator PrintDeltaTime()
    {
        Debug.Log("Delta time: " + Time.deltaTime);
        yield return null;
        Debug.Log("Delta time: " + Time.deltaTime);
    }
    float GetNoiseAtPos(float x, float z)
    {
        // float sum = 0;
        // float frequency = baseFrequency, amplitude = baseAmplitude;
        
        // for(int i = 0; i < octaves.Length; i++)
        
        // { 
        //     seed = Random.Range(0, 10000);
        //     float n = Mathf.PerlinNoise((seed + x) / ( octaves[i].lacunarity / scale), (seed + z) / ( octaves[i].lacunarity / scale));
        //     sum += n * octaves[i].gain;
        //     // frequency *= octaves[i].lacunarity;
        //     // amplitude *= octaves[i].gain;
        // }
       
        float sum = Implement(x, z);
        return sum;
    }

    float Implement(float x, float z)
    {
        
        float amplitude = maxHeight/2;
        float frequency = 0.05f;
        int octaves = this.octaves;

        float elevation = amplitude;
        float totalFrequency = frequency;
        float totalAmplitude = amplitude;

        for(int i = 0; i < octaves; i++)
        {
            float sampleX = x / scale * totalFrequency;
            float sampleZ = z / scale * totalFrequency;
            elevation += Mathf.PerlinNoise(sampleX, sampleZ) * totalAmplitude;
            // Debug.Log("Elevation: " + elevation);
            totalFrequency *= lacunarity;
            totalAmplitude *= gain;
        }
        // elevation = Mathf.Clamp(elevation, minHeight, maxHeight);
        return elevation;
    }

    void Update()
    {
        if(regen)
        {
            CreateMesh();
            ApplyNoise();
            UpdateMesh();
            regen = false;
        }
    }
}