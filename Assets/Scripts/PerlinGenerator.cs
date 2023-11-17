using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    public enum Biome { Grassland, Mountains, Desert, Lake };

    public Biome[,] biomes;
    public int biomeGrowthCycles;
    public float mountainHeightMultiplier;
    public float desertHeightDivision;
    public float seaLevel;
    public float minSpawnDif;
    Vector2 desertSpawn;
    Vector2 mountainSpawn;

    public MeshGenerator meshGenerator;

    // Start is called before the first frame update
    void Start()
    {
        
        GenerateInitialMap();

        GenerateBiomes();

        meshGenerator.GenerateMesh(heightMap);

        Debug.Log("Done");

    }

    //Creates the initial HeightMap
    void GenerateInitialMap()
    {
        heightMap = new float[width, depth];
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                for (int z = 0; z <= spectrals; z++)
                {
                    heightMap[x, y] += Mathf.PerlinNoise((float)x * (Mathf.Pow(spectralScaling, z)) / divisionHandler, (float)y * (Mathf.Pow(spectralScaling, z)) / divisionHandler) * (intensity / (1 + z));
                }
            }
        }
    }

    void GenerateBiomes()
    {
        biomes = new Biome[width, depth];
        for (int x = 0; x < biomes.GetLength(0); x++)
        {
            for (int y = 0; y < biomes.GetLength(1); y++)
            {
                biomes[x, y] = Biome.Grassland;
            }
        }

        desertSpawn = new Vector2(Random.Range(0, width), Random.Range(0, depth));
        do
        {
            mountainSpawn = new Vector2(Random.Range(0, width), Random.Range(0, depth));
        } while (desertSpawn.x - mountainSpawn.x < minSpawnDif && desertSpawn.y - mountainSpawn.y < minSpawnDif);

        Debug.Log(desertSpawn);
        Debug.Log(mountainSpawn);

        biomes[(int)desertSpawn.x, (int)desertSpawn.y] = Biome.Desert;
        biomes[(int)mountainSpawn.x, (int)mountainSpawn.y] = Biome.Mountains;

        for (int i = 0; i < biomeGrowthCycles; i++)
        {
            List<Vector2> toDesert = new List<Vector2>();
            List<Vector2> toMountain = new List<Vector2>();
            for (int x = 0; x < biomes.GetLength(0); x++)
            {
                for (int y = 0; y < biomes.GetLength(1); y++)
                {
                    if (biomes[x, y] == Biome.Grassland)
                    {
                        switch (adjacentToBiome(x, y))
                        {
                            case Biome.Desert:
                                Debug.Log("Desert");
                                //biomes[x, y] = Biome.Desert;
                                toDesert.Add(new Vector2(x, y));
                                break;
                            case Biome.Mountains:
                                Debug.Log("Mountain");
                                //biomes[x, y] = Biome.Mountains;
                                toMountain.Add(new Vector2(x, y));
                                break;
                        }
                    }
                }
            }

            foreach (Vector2 vec in toDesert)
            {
                biomes[(int)vec.x, (int)vec.y] = Biome.Desert;
            }
            foreach (Vector2 vec in toMountain)
            {
                biomes[(int)vec.x, (int)vec.y] = Biome.Mountains;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                switch(biomes[x,y])
                {
                    case Biome.Desert:
                        heightMap[x,y] = heightMap[x,y] / desertHeightDivision;
                        break;
                    case Biome.Mountains:
                        heightMap[x, y] = heightMap[x, y] * mountainHeightMultiplier;
                        break;
                    case Biome.Grassland:
                        if (heightMap[x,y] < seaLevel * intensity)
                        {
                            heightMap[x, y] = seaLevel * intensity;
                            biomes[x, y] = Biome.Lake;
                            Debug.Log("LAKE");
                        }
                        break;
                }
            }
        }

    }

    Biome adjacentToBiome(int x, int y)
    {
        Biome[] temps = new Biome[4];
        if(x > 0)
        {
            temps[0] = biomes[x - 1, y];
        }
        else
        {
            temps[0] = Biome.Grassland;
        }

        if(x < width - 1)
        {
            temps[1] = biomes[x + 1, y];
        }
        else
        {
            temps[1] = Biome.Grassland;
        }

        if(y > 0)
        {
            temps[2] = biomes[x, y - 1];
        }
        else
        {
            temps[2] = Biome.Grassland;
        }

        if(y < depth - 1)
        {
            temps[3] = biomes[x, y + 1];
        }
        else
        {
            temps[3] = Biome.Grassland;
        }

        bool M = temps.Contains(Biome.Mountains);
        bool D = temps.Contains(Biome.Desert);

        if((M && D) || (!M && !D))
        {
            return Biome.Grassland;
        }else if(M){
            return Biome.Mountains;
        }else
        {
            return Biome.Desert;
        }

    }

}
