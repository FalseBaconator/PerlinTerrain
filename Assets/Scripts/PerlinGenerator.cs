using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{

    public int width;
    public int depth;

    public float divisionHandler;
    public float intensity;
    public int spectrals;
    public int spectralScaling;

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
                for (int z = 0; z <= spectrals; z++)
                {
                    Debug.Log(intensity / (1 + z));
                    heightMap[x, y] += Mathf.PerlinNoise((float)x * (Mathf.Pow(spectralScaling, z)) / divisionHandler, (float)y * (Mathf.Pow(spectralScaling, z)) / divisionHandler) * (intensity / (1 + z));
                }
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
