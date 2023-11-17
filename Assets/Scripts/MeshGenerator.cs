using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(float[,] heightMap)
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                vertices.Add(new Vector3(x, heightMap[x, y], y));
                if(x < heightMap.GetLength(0) - 1 && y < heightMap.GetLength(1) - 1)
                {
                    triangles.Add(((y + 1) * heightMap.GetLength(1)) + x);
                    triangles.Add((y * heightMap.GetLength(1)) + x);
                    triangles.Add(((y + 1) * heightMap.GetLength(1)) + x + 1);
                    triangles.Add(((y + 1) * heightMap.GetLength(1)) + x + 1);
                    triangles.Add((y * heightMap.GetLength(1)) + x);
                    triangles.Add((y * heightMap.GetLength(1)) + x + 1);
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

    }




}
