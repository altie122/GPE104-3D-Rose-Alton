using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Terrain terrain;

    [Header("Terrain Dimensions")]
    public int width = 256;
    public int length = 256;
    public int depth = 20;

    [Header("Noise Settings")]
    public float scale = 20f;
    
    private float offsetX;
    private float offsetY;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;

        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, length);

        terrainData.SetHeights(0, 0, CalculateHeights());
    }

    float[,] CalculateHeights()
    {
        float[,] heights = new float[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = CalculateNoiseValue(x, y);
            }
        }

        return heights;
    }

    float CalculateNoiseValue(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / length * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}