using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class TerrainStreamer : MonoBehaviour
{
    [Header("References")] public Transform player;

    public string chunkSceneName = "Terrain Chunk";

    [Header("Chunk Dimensions")] public Vector2Int chunkSize = new Vector2Int(256, 256);

    public float terrainY = 0f;

    [Header("Streaming")] [Min(1)] public int loadRadius = 1;

    [Min(1)] public int keepRadius = 2;

    [Header("Generation")] [InspectorName("Use Random Seed")]
    public bool isUsingRandomSeed = false;

    [DisableIf("isUsingRandomSeed")] 
    public int seed = 12345;

    public bool IsReady { get; private set; }

    private readonly Dictionary<Vector2Int, LoadedChunk> chunks =
        new Dictionary<Vector2Int, LoadedChunk>();

    private Vector2 noiseOffset;
    private bool failed;

    private class LoadedChunk
    {
        public Scene scene;
        public Terrain terrain;
    }

    private void Awake()
    {
        if (isUsingRandomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
    }

    private IEnumerator Start()
    {
        if (player == null)
        {
            Debug.LogError("Assign a player to TerrainStreamer.", this);
            yield break;
        }

        if (chunkSize.x <= 0 || chunkSize.y <= 0)
        {
            Debug.LogError("Chunk dimensions must be positive.", this);
            yield break;
        }

        if (!Application.CanStreamedLevelBeLoaded(chunkSceneName))
        {
            Debug.LogError(
                "Add the terrain chunk scene to the build scene list.",
                this
            );
            yield break;
        }

        loadRadius = Mathf.Max(1, loadRadius);
        keepRadius = Mathf.Max(loadRadius, keepRadius);

        System.Random random = new System.Random(seed);

        noiseOffset = new Vector2(
            (float)random.NextDouble() * 10000f,
            (float)random.NextDouble() * 10000f
        );

        while (player != null)
        {
            Vector2Int playerChunk = WorldToChunk(player.position);

            // Remove one distant chunk, then check the player's
            // position again after the asynchronous unload.
            if (TryGetDistantChunk(playerChunk, out Vector2Int oldCoord))
            {
                LoadedChunk oldChunk = chunks[oldCoord];
                chunks.Remove(oldCoord);

                oldChunk.terrain.SetNeighbors(null, null, null, null);
                RefreshNeighbors();

                yield return SceneManager.UnloadSceneAsync(
                    oldChunk.scene
                );

                continue;
            }

            // Load one missing chunk at a time, starting near the player.
            if (
                TryGetMissingChunk(
                    playerChunk,
                    out Vector2Int nextCoord
                )
            )
            {
                yield return LoadChunk(nextCoord);

                if (failed)
                {
                    yield break;
                }

                continue;
            }

            IsReady = true;
            yield return null;
        }
    }

    private Vector2Int WorldToChunk(Vector3 position)
    {
        // Chunk (0, 0) is centered around world X/Z = 0.
        // FloorToInt also handles negative coordinates correctly.
        int x = Mathf.FloorToInt(
            (position.x + chunkSize.x * 0.5f) / chunkSize.x
        );

        int z = Mathf.FloorToInt(
            (position.z + chunkSize.y * 0.5f) / chunkSize.y
        );

        return new Vector2Int(x, z);
    }

    private Vector3 ChunkToWorld(Vector2Int coordinate)
    {
        return new Vector3(
            (coordinate.x - 0.5f) * chunkSize.x,
            terrainY,
            (coordinate.y - 0.5f) * chunkSize.y
        );
    }

    private bool TryGetMissingChunk(
        Vector2Int center,
        out Vector2Int coordinate
    )
    {
        // Search outward in square rings, loading the current
        // chunk before its surroundings.
        for (int radius = 0; radius <= loadRadius; radius++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    bool isOnRing =
                        Mathf.Max(Mathf.Abs(x), Mathf.Abs(z)) == radius;

                    if (!isOnRing)
                    {
                        continue;
                    }

                    Vector2Int candidate =
                        center + new Vector2Int(x, z);

                    if (!chunks.ContainsKey(candidate))
                    {
                        coordinate = candidate;
                        return true;
                    }
                }
            }
        }

        coordinate = default;
        return false;
    }

    private bool TryGetDistantChunk(
        Vector2Int center,
        out Vector2Int coordinate
    )
    {
        foreach (Vector2Int candidate in chunks.Keys)
        {
            int distanceX = Mathf.Abs(candidate.x - center.x);
            int distanceZ = Mathf.Abs(candidate.y - center.y);

            if (distanceX > keepRadius || distanceZ > keepRadius)
            {
                coordinate = candidate;
                return true;
            }
        }

        coordinate = default;
        return false;
    }

    private IEnumerator LoadChunk(Vector2Int coordinate)
    {
        Scene loadedScene = default;

        void CaptureScene(Scene scene, LoadSceneMode mode)
        {
            if (
                mode == LoadSceneMode.Additive &&
                scene.name == chunkSceneName
            )
            {
                loadedScene = scene;
            }
        }

        SceneManager.sceneLoaded += CaptureScene;

        try
        {
            yield return SceneManager.LoadSceneAsync(
                chunkSceneName,
                LoadSceneMode.Additive
            );
        }
        finally
        {
            SceneManager.sceneLoaded -= CaptureScene;
        }

        if (!loadedScene.IsValid())
        {
            Debug.LogError("Could not identify the loaded chunk.", this);
            failed = true;
            yield break;
        }

        GameObject[] roots = loadedScene.GetRootGameObjects();

        TerrainGenerator generator = roots.Length == 1
            ? roots[0].GetComponentInChildren<TerrainGenerator>()
            : null;

        if (
            generator == null ||
            generator.width != chunkSize.x ||
            generator.length != chunkSize.y
        )
        {
            Debug.LogError(
                "The chunk needs one root containing a TerrainGenerator. " +
                "Its width and length must match the streamer's Chunk Size.",
                this
            );

            failed = true;

            yield return SceneManager.UnloadSceneAsync(loadedScene);
            yield break;
        }

        GameObject chunkRoot = roots[0];

        chunkRoot.transform.position = ChunkToWorld(coordinate);

        generator.GenerateTerrain(noiseOffset);

        TerrainSpawnPointPlacer placer =
            chunkRoot.GetComponent<TerrainSpawnPointPlacer>();

        Chunk level = chunkRoot.GetComponent<Chunk>();

        if (placer != null && level != null)
        {
            placer.PlaceOnTerrain(generator.terrain);
            level.SpawnObjects();
        }
        else
        {
            Debug.LogError(
                "ChunkRoot needs TerrainSpawnPointPlacer and Level components.",
                chunkRoot
            );
        }

        chunks.Add(
            coordinate,
            new LoadedChunk
            {
                scene = loadedScene,
                terrain = generator.terrain
            }
        );

        RefreshNeighbors();
    }

    private Terrain GetTerrain(Vector2Int coordinate)
    {
        return chunks.TryGetValue(coordinate, out LoadedChunk chunk)
            ? chunk.terrain
            : null;
    }

    private void RefreshNeighbors()
    {
        foreach (
            KeyValuePair<Vector2Int, LoadedChunk> entry in chunks
        )
        {
            Vector2Int coordinate = entry.Key;

            Terrain left = GetTerrain(
                coordinate + Vector2Int.left
            );

            Terrain ahead = GetTerrain(
                coordinate + Vector2Int.up
            );

            Terrain right = GetTerrain(
                coordinate + Vector2Int.right
            );

            Terrain behind = GetTerrain(
                coordinate + Vector2Int.down
            );

            entry.Value.terrain.SetNeighbors(
                left,
                ahead,
                right,
                behind
            );
        }
    }
}