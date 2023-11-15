using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{

    public int width;
    public int depth;

    public float[,] heightMap;

    public MeshGenerator meshGenerator;

    // Start is called before the first frame update
    void Start()
    {
        heightMap = new float[width,depth];
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                heightMap[x, y] = Mathf.PerlinNoise((float)x/10, (float)y/10);
                heightMap[x, y] += Mathf.PerlinNoise((float)x * 2 / 10, (float)y * 2 / 10) / 5;
                
            }
        }

        meshGenerator.GenerateMesh(heightMap);

        /*for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x, heightMap[x,y] * 5, y);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
