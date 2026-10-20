using UnityEngine;

public enum SpawnEntityType
{
    HealthPack,
    Astronaut,
    UFO
}

public class Spawner : MonoBehaviour
{
    public SpawnEntityType entityType;
    public GameObject[] prefabs;

    private bool hasSpawned;

    public bool CanSpawn
    {
        get
        {
            if (hasSpawned || !isActiveAndEnabled || prefabs == null)
            {
                return false;
            }

            return System.Array.Exists(
                prefabs,
                prefab => prefab != null
            );
        }
    }

    public bool Spawn()
    {
        if (!CanSpawn)
        {
            return false;
        }

        GameObject[] validPrefabs = System.Array.FindAll(
            prefabs,
            prefab => prefab != null
        );

        int index = Random.Range(0, validPrefabs.Length);
        GameObject selectedPrefab = validPrefabs[index];

        hasSpawned = true;

        Instantiate(
            selectedPrefab,
            transform.position,
            transform.rotation,
            transform
        );

        return true;
    }
}