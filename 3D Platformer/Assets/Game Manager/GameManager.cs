using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game events
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Internal reference to active GameManager instance
    /// </summary>
    private static GameManager _instance;
    /// <summary>
    /// The active GameManager instance
    /// </summary>
    public static GameManager Instance { get { return _instance; } }

    // enums
    /// <summary>
    /// States of the game
    /// </summary>
    public enum GameState
    {
        Menu, Playing, Credits
    }
    /// <summary>
    /// Pause states of the game
    /// </summary>
    public enum PauseState
    {
        Playing, Paused
    }

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState CurrentGameState { get; private set; }

    /// <summary>
    /// The current pause state of the game
    /// </summary>
    public PauseState CurrentPauseState { get; private set; }

    // Events for game pausing
    [Tooltip("Triggers when the game is paused")]
    public UnityEvent OnGamePaused;
    [Tooltip("Triggers when the game is resumed")]
    public UnityEvent OnGameResumed;

    /// <summary>
    /// The amount of health the player currently has
    /// </summary>
    public int Health { get; private set; }

    // Event for lives changing
    [Tooltip("Triggers when HP is changed")]
    public UnityEvent OnHealthChanged;

    // Event for game over
    [Tooltip("Triggers when HP is 0 (game over)")]
    public UnityEvent OnGameOver;

    // Run on instance being loaded
    void Awake()
    {
        // Make sure instance is null
        if (_instance == null)
        {
            // Set this GameManager as current instance
            _instance = this;

            // Preserve between scenes
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Another GameManager already exists, remove this one
            Destroy(this.gameObject);
        }

        // Initial health
        Health = 10;
    }

    /// <summary>
    /// Actions to take upon a new scene being loaded
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Resume game
        ResumeGame();
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        // Pause game
        Time.timeScale = 0f;

        // Set current game state
        CurrentPauseState = PauseState.Paused;

        // Notify listening objects
        OnGamePaused.Invoke();
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    public void ResumeGame()
    {
        // Resume game
        Time.timeScale = 1f;

        // Set current game state
        CurrentPauseState = PauseState.Playing;

        // Notify listening objects
        OnGameResumed.Invoke();
    }

    /// <summary>
    /// Changes the current scene of the game
    /// </summary>
    /// <param name="newScene">The name of the scene to change to</param>
    public void ChangeScene(string newScene)
    {
        // Change scene
        SceneManager.LoadScene(newScene);

        // Set game state depending on scene type loaded
        switch (newScene)
        {
            case string x when x.StartsWith("Level"): // Game level
                CurrentGameState = GameState.Playing;
                ResumeGame();
                break;
            case "Credits": // End credits
                CurrentGameState = GameState.Credits;
                break;
            default: // Menu
                CurrentGameState = GameState.Menu;
                break;
        }
    }

    /// <summary>
    /// Adds HP when called
    /// </summary>
    public void AddHealth(int amount)
    {
        // Add health
        Health += amount;

        // Notify listening objects
        OnHealthChanged.Invoke();
    }

    /// <summary>
    /// Removes HP when called; also checks for game over
    /// </summary>
    public void RemoveHealth(int amount)
    {
        // Remove health
        Health -= amount;

        // Check if game over
        if (Health <= 0)
        {
            // Set GameOver state
            GameOver();
        }

        // Notify listening objects
        OnHealthChanged.Invoke();
    }

    /// <summary>
    /// Called when a Game Over occurs
    /// </summary>
    public void GameOver()
    {
        // Invoke event
        OnGameOver.Invoke();
    }
}
