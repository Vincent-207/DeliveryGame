using UnityEngine;

public class AnalyticalDerivative : MeshGen
{
    public int octaves;
    void Start()
    {
        CreateMesh();
        
    }


    void ApplyNoise()
    {
        float sum = 0.5f, frequency = 1f, amplitude = 1f;
        Vector2 dSum = Vector2.zero;

        for(int i = 0; i < octaves; i++)
        {
            
        }
    }
}
