using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Script used to control the HUD
/// </summary>
public class HUDController : MonoBehaviour
{
    #region Serialized fields
    [Tooltip("The UI Document used for the HUD")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The Timer used for level time remaining")]
    [SerializeField] private Timer levelTimer;
    [Tooltip("The sprite used to represent health/HP in the HUD")]
    [SerializeField] private Sprite healthSprite;
    [Tooltip("The size of a health/HP sprite")]
    [SerializeField] private float healthSpriteSize = 16;
    #endregion

    #region Private fields
    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;
    /// <summary>
    /// Container used for displaying health
    /// </summary>
    private VisualElement healthContainer;
    /// <summary>
    /// Label used to display time
    /// </summary>
    private Label timerDisplay;
    /// <summary>
    /// Caches the previous HP state for comparison
    /// </summary>
    private int healthCache;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;

        // Get UI elements
        VisualElement root = UIDoc.rootVisualElement;
        healthContainer = root.Q<VisualElement>("health");
        timerDisplay = root.Q<Label>("timerLabel");
        if (healthContainer == null)
        {
            Debug.Log("Could not find health display");
        }
        if (timerDisplay == null)
        {
            Debug.Log("Could not find timer display");
        }

        // Initial health
        for (int i = 0; i < gameManager.Health; i++)
        {
            AddHealth();
        }

        // Cache initial health
        healthCache = gameManager.Health;

        // Subscribe to Unity and UI events
        SubscribeToEvents();
    }

    // OnDestroy is called before this script instance is removed
    void OnDestroy()
    {
        // Unsubscribe from Unity and UI events
        UnsubscribeFromEvents();

        // Stop timer
        levelTimer.ResetTimer();
    }
    #endregion

    #region Event subscription
    /// <summary>
    /// Subscribes this script instance to Unity & UI events
    /// </summary>
    private void SubscribeToEvents()
    {
        // GameManager events
        gameManager.OnHealthChanged.AddListener(OnHealthUpdate);
    }

    /// <summary>
    /// Unsubscribes this script instance from Unity & UI events
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        // GameManager events
        gameManager.OnHealthChanged.RemoveListener(OnHealthUpdate);
    }
    #endregion

    #region Event handling
    private void OnHealthUpdate()
    {
        // Get updated health value
        int newHealthCount = gameManager.Health;

        // Compare to previous health value
        if (newHealthCount >= healthCache)
        {
            // Add additonal health
            for (int i = 0; i < Mathf.Abs(newHealthCount - healthCache); i++)
            {
                AddHealth();
            }
        }
        else
        {
            // Remove lost health
            for (int i = 0; i < Mathf.Abs(newHealthCount - healthCache); i++)
            {
                RemoveHealth();
            }
        }

        // Update health cache
        healthCache = newHealthCount;
    }

    #region Level timer
    /// <summary>
    /// Called once per level timer tick; should listen for tick event
    /// </summary>
    public void LevelTimerTick()
    {
        // Update timer display
        timerDisplay.text = "Time: " + Mathf.RoundToInt(levelTimer.TimeLeft());
    }

    /// <summary>
    /// Called when timer has expired; should listen for timer expiry event
    /// </summary>
    public void LevelTimerExpired()
    {
        // Game is over
        gameManager.GameOver();
    }
    #endregion
    #endregion

    #region Health display changing
    /// <summary>
    /// Adds an additional health sprite to the HUD
    /// </summary>
    private void AddHealth()
    {
        // Set health image
        Image healthImage = new Image();
        healthImage.sprite = healthSprite;

        // Set padding
        healthImage.style.paddingTop = 5;
        healthImage.style.paddingLeft = 0;
        healthImage.style.paddingRight = 0;
        healthImage.style.paddingBottom = 0;

        // Set size
        healthImage.style.width = healthSpriteSize;
        healthImage.style.height = healthSpriteSize;

        // Add to container
        healthContainer.Add(healthImage);
    }

    /// <summary>
    /// Removes a health sprite from the HUD
    /// </summary>
    private void RemoveHealth()
    {
        // Remove oldest health sprite from container
        healthContainer.RemoveAt(0);
    }
    #endregion
}
