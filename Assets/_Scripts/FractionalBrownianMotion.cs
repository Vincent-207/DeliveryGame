using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class FractionalBrownianMotion : MeshGen
{
    public Octave[] octaves;
    public float baseFrequency, baseAmplitude;
    public float lacunarity, gain;
    public bool regen;
    public int seed;
    // I'm not sure what lacunarity is, but here it is from his talk. 
    // Lacunarity, from the Latin lacuna, meaning "gap" or "lake", is a specialized term in geometry referring to a measure of how patterns, especially fractals, fill space, where patterns having more or larger gaps generally have higher lacunarity.

    void Start()
    {
        CreateMesh();
        seed = Random.Range(1, 1000000);
        ApplyNoise();
        UpdateMesh();
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
    float GetNoiseAtPos(float x, float z)
    {
        float sum = 0;
        float frequency = baseFrequency, amplitude = baseAmplitude;

        for(int i = 0; i < octaves.Length; i++)
        {
            float n = Mathf.PerlinNoise((seed + x) / frequency, (seed + z) / frequency);
            sum += n * amplitude;
            frequency *= lacunarity;
            amplitude *= gain;
        }

        return sum;
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


[System.Serializable]
public class Octave
{
    public float lacunarity, gain;
}