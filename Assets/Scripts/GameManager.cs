using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
    public TMP_Text livesText;
    
    [Header("Audio")]
    public AudioMixer audioMixer;

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
        // Check if the game is over
        if (isGameOver)
        {
            // If the game is over, check if the game should be restarted
            if (playerController && playerController.controlledPawn)
            {
                // If the game is ready to be started, unpause and restart game
                isPaused = false;
                isPauseMenuEnabled = true;
                isGameOver = false;
                Time.timeScale = 1f;
            }
            // If the game is over, return early
            return;
        }
        // If the game is not over, continue normal checks and processes
        else
        {
            if (playerController && !playerController.controlledPawn && !LevelData.instance.isPlayerAlive)
            {
                LoseGame();
            }
        
            // Update score text
            if (scoreText)
            {
                scoreText.text = "Score: " + score;
            }
            
            // Update lives text
            if (livesText)
            {
                if (LevelData.instance)
                {
                    livesText.text = "Lives: " + LevelData.instance.playerLives;
                }
                else
                {
                    livesText.text = "Waiting on lives count";
                }
            }
        }
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
        
        playerGameObject = null;
        
        obstacleList.Clear();
        
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
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
