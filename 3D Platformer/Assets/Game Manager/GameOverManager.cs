using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles game over logic for the GameManager; should be attached to
/// GameManager for persistence
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Tooltip("How long to delay changing to game over scene")]
    [SerializeField] private float sceneChangeDelay = 10f;

    /// <summary>
    /// The active game manager instance
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// The time that has passed since the initial OnGameOver invoke
    /// </summary>
    private float time = 0f;

    /// <summary>
    /// Whether the delay timer has been triggered
    /// </summary>
    private bool delayActive = false;

    /// <summary>
    /// Performs behavior upon GameManager.OnGameOver being invoked
    /// </summary>
    private void OnGameOver()
    {
        // Set to active
        delayActive = true;
    }

    #region Unity Functions
    // Called before first frame update
    void Start()
    {
        // Get game manager instance
        gameManager = GameManager.Instance;

        // Listen for OnGameOver event
        gameManager.OnGameOver.AddListener(OnGameOver);
    }

    // Called every frame
    void Update()
    {
        // Only do anything if active
        if (delayActive)
        {
            // Increment timer
            time += Time.deltaTime;

            // Determine if timer has expired
            if (time >= sceneChangeDelay)
            {
                // Reset to default health
                gameManager.AddHealth(10);

                // Change scene
                gameManager.ChangeScene("GameOver");

                // Reset timer
                delayActive = false;
                time = 0f;
            }
        }
    }

    // Called upon instance removal
    void OnDestroy()
    {
        // Stop listening for OnGameOver event
        gameManager.OnGameOver.RemoveListener(OnGameOver);
    }
    #endregion
}
