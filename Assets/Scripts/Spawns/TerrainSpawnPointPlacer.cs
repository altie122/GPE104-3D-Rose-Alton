using UnityEngine;

public class TerrainSpawnPointPlacer : MonoBehaviour
{
    public Transform spawnPointContainer;

    public float edgeMargin = 8f;

    public Vector2 heightAboveGround = new Vector2(3f, 8f);

    public void PlaceOnTerrain(Terrain terrain)
    {
        if (terrain == null || spawnPointContainer == null)
        {
            Debug.LogError(
                "Assign a terrain and a spawn point container.",
                this
            );

            return;
        }

        Vector3 origin = terrain.transform.position;
        Vector3 size = terrain.terrainData.size;

        float marginX = Mathf.Clamp(edgeMargin, 0f, size.x * 0.5f);
        float marginZ = Mathf.Clamp(edgeMargin, 0f, size.z * 0.5f);

        float minimumHeight = Mathf.Min(
            heightAboveGround.x,
            heightAboveGround.y
        );

        float maximumHeight = Mathf.Max(
            heightAboveGround.x,
            heightAboveGround.y
        );

        foreach (Transform spawnPoint in spawnPointContainer)
        {
            float worldX = origin.x + Random.Range(
                marginX,
                size.x - marginX
            );

            float worldZ = origin.z + Random.Range(
                marginZ,
                size.z - marginZ
            );

            Vector3 position = new Vector3(worldX, 0f, worldZ);

            float groundHeight =
                terrain.SampleHeight(position) + origin.y;

            position.y = groundHeight + Random.Range(
                minimumHeight,
                maximumHeight
            );

            spawnPoint.position = position;
        }
    }
}