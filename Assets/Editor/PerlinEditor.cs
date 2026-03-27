using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinGPU))]
public class PerlinEditor : Editor
{
    PerlinGPU perlin;
    ComputeShader perlinNoise;
    int perlinNoiseHandle;
    int resolution = 64;
    public RenderTexture texture;
    private void OnEnable()
    {
        Generate();
    }
    

    public void Generate()
    {
        perlin = (PerlinGPU)target;
        // is resolution being set as w and h?
        texture = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.RFloat)
        {
            enableRandomWrite = true
        };
        texture.Create();

        // ComputeBuffer gradients = new ComputeBuffer(256, sizeof(float) * 2);
	    // gradients.SetData(Enumerable.Range(0, 256).Select((i) => GetRandomDirection()).ToArray());

        perlinNoise = (ComputeShader)Resources.Load("PerlinNoise");
        perlinNoiseHandle = perlinNoise.FindKernel("CSMain");
        perlinNoise.SetTexture(perlinNoiseHandle, "Result", texture);
        perlinNoise.SetFloat("res", (float) resolution);
        perlinNoise.SetFloat("xOffset", perlin.transform.position.x);
        perlinNoise.SetFloat("yOffset", perlin.transform.position.y);
        //perlinNoise.SetBuffer(perlinNoiseHandle, "gradients", gradients);
        perlinNoise.Dispatch(perlinNoiseHandle, resolution/8, resolution/8, 1);


        Renderer rend = perlin.GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial.SetTexture("_Texture2D", texture);
    }

    private void OnSceneGUI()
    {
        
    }
    private void OnDisable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField("Texture", texture, typeof(Texture), false);
        // EditorGUILayout.FloatField("Scale", scale);
        // if(GUILayout.Button("Generate"))
        // {
            
        // }
        EditorGUILayout.EndHorizontal();

    }
}
