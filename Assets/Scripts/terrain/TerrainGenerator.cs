using UnityEngine;

[RequireComponent(typeof(Terrain), typeof(TerrainCollider))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Dimensions")]
    [Min(1)]
    public int width = 256;

    [Min(1)]
    public int length = 256;

    [Min(1)]
    public int depth = 20;

    [Header("Heightmap")]
    public int heightmapResolution = 257;

    [Header("Noise Settings")]
    [Min(0.001f)]
    public float scale = 20f;

    public Terrain terrain;
    private TerrainData runtimeData;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    public void GenerateTerrain(Vector2 noiseOffset)
    {
        if (runtimeData == null)
        {
            // Each loaded chunk needs its own editable terrain data.
            runtimeData = Instantiate(terrain.terrainData);
            runtimeData.name = "Runtime Terrain Data";
        }

        runtimeData.heightmapResolution = heightmapResolution;
        runtimeData.size = new Vector3(width, depth, length);

        // Read Unity's actual resolution in case it clamped the setting.
        int resolution = runtimeData.heightmapResolution;

        runtimeData.SetHeights(
            0,
            0,
            CalculateHeights(resolution, noiseOffset)
        );

        terrain.terrainData = runtimeData;

        TerrainCollider terrainCollider = GetComponent<TerrainCollider>();
        terrainCollider.terrainData = runtimeData;
    }

    private float[,] CalculateHeights(
        int resolution,
        Vector2 noiseOffset
    )
    {
        float[,] heights = new float[resolution, resolution];
        Vector3 origin = transform.position;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float normalizedX = (float)x / (resolution - 1);
                float normalizedZ = (float)z / (resolution - 1);

                float worldX = origin.x + normalizedX * width;
                float worldZ = origin.z + normalizedZ * length;

                float noiseX = worldX / width * scale + noiseOffset.x;
                float noiseZ = worldZ / length * scale + noiseOffset.y;

                heights[z, x] = Mathf.PerlinNoise(noiseX, noiseZ);
            }
        }

        return heights;
    }

    private void OnDestroy()
    {
        // Runtime-created TerrainData must also be cleaned up.
        if (runtimeData != null)
        {
            Destroy(runtimeData);
        }
    }
}