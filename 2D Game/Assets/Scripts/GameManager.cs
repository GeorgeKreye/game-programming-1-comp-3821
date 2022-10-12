using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    /// <summary>
    /// The active GameManager instance
    /// </summary>
    public static GameManager Instance { get { return _instance; } }

    /// <summary>
    /// States of the game
    /// </summary>
    public enum GameState
    {
        Menu, Playing, Paused
    }

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState CurrentGameState { get; private set; }

    // Event for game start


    // Events for game pausing
    [Tooltip("Triggers when the game is paused")]
    public UnityEvent OnGamePaused;
    [Tooltip("Triggers when the game is resumed")]
    public UnityEvent OnGameResumed;


    /// <summary>
    /// The amount of lives the player currently has
    /// </summary>
    public int lives { get; private set; }

    // Event for lives updates
    [Tooltip("Triggers when lives count is updated")]
    public UnityEvent OnLivesChanged;

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

        // Initial lives
        lives = 3;
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
        CurrentGameState = GameState.Paused;

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
        CurrentGameState = GameState.Playing;

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
    }

    public void AddLife()
    {
        // Add life
        lives++;

        // Notify listening objects
        OnLivesChanged.Invoke();
    }

    public void RemoveLife()
    {
        // Remove life
        lives--;

        // Check if game over
        if (lives <= 0)
        {
            GameOver.Invoke();
        }

        // Notify listening objects
        OnLivesChanged.Invoke();
    }
}
