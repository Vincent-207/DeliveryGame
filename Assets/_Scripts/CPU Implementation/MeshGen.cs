using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(MeshFilter))]
public class MeshGen : MonoBehaviour
{
    public int sideLength;
    MeshFilter meshFilter;
    internal Vector3[] verticies;
    internal int[] triangles;
    internal Mesh mesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    internal void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new();
        meshFilter.mesh = mesh;
        verticies = CreateVerticies(sideLength);
        triangles = CreateTriangles();
        UpdateMesh();
    }
    internal void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    int[] CreateTriangles()
    {
        int squareCount = (sideLength * sideLength);
        int[] triangles = new int[squareCount * 6];
        for(int vert = 0, tris = 0, z = 0; z < sideLength; z++)
        {
            for(int x = 0; x < sideLength; x++)
            {
                
                triangles[tris + 0] = vert + 0;
                triangles[tris + 2] = vert + sideLength + 1;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 3] = vert + sideLength + 1;
                triangles[tris + 4] = vert + 1;
                triangles[tris + 5] = vert + sideLength + 2;
                
                vert++;
                tris += 6;
            }
            vert++;
        }
        return triangles;
    }

    Vector3[] CreateVerticies(int sideLength)
    {
        Vector3[] verticies = new Vector3[(sideLength +1) * (sideLength +1)];
        
        for(int i =0, x = 0; x <= sideLength; x++)
        {
            for(int z = 0; z <= sideLength; z++)
            {
                verticies[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        return verticies;
    }
}
