using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    
    [Header("Game pausing")]
    public bool isPaused;
    public bool isPauseMenuEnabled = true;
    
    [Header("Game components")]
    public PlayerController playerController;
    public GameObject playerGameObject;
    public List<Obstacle> obstacleList;
    
    [Header("Game over")]
    public EndScreen endScreen;
    public bool isGameOver;
    
    [Header("Score")]
    public int score;
    public TMP_Text scoreText;
    
    [Header("Lives")]
    public int playerLiveCount;
    public int defaultPlayerLiveCount;
    public TMP_Text livesText;
    
    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("World boundaries")]
    public float worldBoundaryMinX;
    public float worldBoundaryMaxX;
    public float worldBoundaryMinY;
    public float worldBoundaryMaxY;

    [Header("Enemies")]
    public List<GameObject> asteroidPrefabs;
    public List<GameObject> ufoPrefabs;
    [Min(1)]
    public int initialEnemyCount;
    [Range(0, 1)]
    public float chanceOfUfo;

    private void Awake()
    {
        // Make sure that onely one GameManager can exist, and to not Destroy it.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Destroy all other instances of GameManager
        else
        {
            Destroy(gameObject);
        }
        
        // Init obstacle list
        obstacleList = new List<Obstacle>();
    }

    private void Update()
    {
        // // Check if the game is over
        // if (isGameOver)
        // {
        //     // If the game is over, check if the game should be restarted
        //     if (playerController && playerController.controlledPawn && obstacleList.Count > 0)
        //     {
        //         // If the game is ready to be started, unpause and restart game
        //         isPaused = false;
        //         isPauseMenuEnabled = true;
        //         isGameOver = false;
        //         Time.timeScale = 1f;
        //     }
        //     // If the game is over, return early
        //     return;
        // }
        // // If the game is not over, continue normal checks and processes
        // else
        // {
        //     // If all obstacles are destroyed and the player's pawn still exists, the game has been won
        //     if (obstacleList != null && playerController && obstacleList.Count <= 0 && playerController.controlledPawn)
        //     {
        //         WinGame();
        //     }
        //     // If the player pawn is destroyed, the game has been lost
        //     else if (playerController && !playerController.controlledPawn)
        //     {
        //         LoseGame();
        //     }
        //
        //     // Update score text
        //     if (scoreText)
        //     {
        //         scoreText.text = "Score: " + score;
        //     }
        //     
        //     // Update lives text
        //     if (livesText)
        //     {
        //         livesText.text = "Lives: " + playerLiveCount;
        //     }
        // }
    }

    // Reset game
    public void ResetGameState()
    {
        Debug.Log("GameManager: Reset Game State");
        score = 0;
        scoreText = null;
        livesText = null;

        playerController = null;
        endScreen = null;
        
        playerLiveCount = defaultPlayerLiveCount;
        
        playerGameObject = null;
        
        obstacleList.Clear();
        
        // Create enemies based on number supplied in `initialEnemyCount`
        for (int index = 0; index < initialEnemyCount; index++)
        {
            // Generate random number 0-1 (represents 0% - 100%)
            float randomNumber = Random.Range(0f, 1f);
            // If the random number is smaller than the chance of an ufo spawning, spawn a UFO
            if (randomNumber <= chanceOfUfo)
            {
                Instantiate(ufoPrefabs[Random.Range(0, ufoPrefabs.Count)], new Vector3(Random.Range(worldBoundaryMinX, worldBoundaryMaxX), Random.Range(worldBoundaryMinY, worldBoundaryMaxY), 0f), Quaternion.identity);
            }
            // If the random number is grater than the chance of an ufo spawning, spawn an asteroid
            else
            {
                Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], new Vector3(Random.Range(worldBoundaryMinX, worldBoundaryMaxX), Random.Range(worldBoundaryMinY, worldBoundaryMaxY), 0f), Quaternion.identity);
            }
        }
    }

    // Lose game
    private void LoseGame()
    {
        isPauseMenuEnabled = false;
        Debug.Log("GameManager: You lose!");
        if (endScreen)
        { 
            endScreen.ShowEndScreen("Fail!", "");
        } 
        isPaused = true;
        Time.timeScale = 0f;
        isGameOver = true;
    }

    // Win game
    private void WinGame()
    {
        isPauseMenuEnabled = false;
        Debug.Log("GameManager: You win!");
        if (endScreen)
        {
            endScreen.ShowEndScreen("Victory!", "Score: "  + score);
        }
        isPaused = true;
        Time.timeScale = 0f;
        isGameOver = true;
    }
    
    // Set audio volume (used in settings)
    public void SetAudioVolume(string parameter, float lowValue, float highValue, float value)
    {
        // If the audio mixer doesn't exist, error and return early
        if (audioMixer == null)
        {
            Debug.LogError(
                "GameManager: Assign audioMixer before changing volume.",
                this
            );
            return;
        }

        // Normalize the value to the high and low values
        float normalizedVolume = Mathf.InverseLerp(
            lowValue,
            highValue,
            value
        );
        
        // Get decibels
        float decibels = normalizedVolume <= 0f
            ? -80f
            : Mathf.Max(-80f, 20f * Mathf.Log10(normalizedVolume));

        // Set volumes
        audioMixer.SetFloat(parameter, decibels);
    }
}
