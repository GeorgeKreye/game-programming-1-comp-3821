using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    /// <summary>
    /// Pause states of the game
    /// </summary>
    public enum GameState
    {
        Playing, Paused
    }

    /// <summary>
    /// The current pause state of the game
    /// </summary>
    public GameState CurrentGameState { get; private set; }

    // Events for game pausing
    [Tooltip("Triggers when the game is paused")]
    public UnityEvent OnGamePaused;
    [Tooltip("Triggers when the game is resumed")]
    public UnityEvent OnGameResumed;

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
}
