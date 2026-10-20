using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int healthPackCount = 2;
    public int astronautCount = 1;
    public int ufoCount = 3;

    private Spawner[] spawnPoints;
    private bool hasSpawnedObjects;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Spawner>(true);
    }

    public void SpawnObjects()
    {
        if (hasSpawnedObjects)
        {
            return;
        }

        hasSpawnedObjects = true;

        SpawnType(SpawnEntityType.HealthPack, healthPackCount);
        SpawnType(SpawnEntityType.Astronaut, astronautCount);
        SpawnType(SpawnEntityType.UFO, ufoCount);
    }

    private void SpawnType(SpawnEntityType type, int requestedCount)
    {
        requestedCount = Mathf.Max(0, requestedCount);

        List<Spawner> availablePoints = new List<Spawner>();

        foreach (Spawner point in spawnPoints)
        {
            if (
                point != null &&
                point.entityType == type &&
                point.CanSpawn
            )
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count < requestedCount)
        {
            Debug.LogWarning(
                $"{name}: Requested {requestedCount} {type} spawns, " +
                $"but only {availablePoints.Count} points are usable.",
                this
            );
        }

        int spawnCount = Mathf.Min(
            requestedCount,
            availablePoints.Count
        );

        for (int i = 0; i < spawnCount; i++)
        {
            int index = Random.Range(0, availablePoints.Count);

            Spawner selectedPoint = availablePoints[index];

            // Removing the point prevents selecting it twice.
            availablePoints.RemoveAt(index);

            selectedPoint.Spawn();
        }
    }
}