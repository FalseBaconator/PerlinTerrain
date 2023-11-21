using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    List<Vector3> vertices;
    List<int> triangles;

    public Material grassMaterial;
    public Material sandMaterial;
    public Material stoneMaterial;
    public Material waterMaterial;

    public Color waterColor;
    public Color sandColor;
    public Color grassColor;
    public Color stoneColor;

    public float waterToSand;
    public float sandToGrass;
    public float grassToStone;

    Texture2D tex;

    public void GenerateMesh(float[,] heightMap)
    {
        Mesh mesh = new Mesh();
        tex = new Texture2D(heightMap.GetLength(0), heightMap.GetLength(1));
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {

                if (heightMap[x,y] <= waterToSand)
                {
                    tex.SetPixel(x, y, waterColor);
                }else if (heightMap[x,y] <= sandToGrass)
                {
                    tex.SetPixel(x, y, sandColor);
                }else if (heightMap[x,y] <= grassToStone)
                {
                    tex.SetPixel(x, y, grassColor);
                }
                else
                {
                    tex.SetPixel(x, y, stoneColor);
                }
                /*switch (biomeMap[x, y])
                {
                    case PerlinGenerator.Biome.Lake:
                        Debug.Log("L");
                        tex.SetPixel(x, y, waterColor);
                        break;
                    case PerlinGenerator.Biome.Desert:
                        Debug.Log("D");
                        tex.SetPixel(x, y, sandColor);
                        break;
                    case PerlinGenerator.Biome.Grassland:
                        Debug.Log("G");
                        tex.SetPixel(x, y, grassColor);
                        break;
                    case PerlinGenerator.Biome.Mountains:
                        Debug.Log("M");
                        tex.SetPixel(x, y, stoneColor);
                        break;
                }*/

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

        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        tex.Apply();
        gameObject.GetComponent<MeshRenderer>().material.mainTexture = tex;

    }




}
