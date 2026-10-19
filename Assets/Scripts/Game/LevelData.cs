using System;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData instance;

    public bool isPlayerAlive = true;
    public int playerLives = 3;
    
    private void Awake()
    {
        // Make sure that only one LevelData can exist.
        if (instance == null)
        {
            instance = this;
        }
        // Destroy all other instances of LevelData
        else
        {
            Destroy(gameObject);
        }
    }
}
