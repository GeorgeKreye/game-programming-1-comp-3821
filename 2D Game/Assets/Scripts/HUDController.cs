using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Tooltip("The UI Document used by the HUD")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The Timer used for level time")]
    [SerializeField] private Timer levelTimer;
    [Tooltip("The sprite used to represent a player life")]
    [SerializeField] private Sprite lifeImage;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    // HUD elements to modify
    /// <summary>
    /// The VisualElement containing lives
    /// </summary>
    private VisualElement livesContainer;
    /// <summary>
    /// The Label displaying the timer
    /// </summary>
    private Label timerLabel;

    /// <summary>
    /// Caches the previous lives state as to allow comparison
    /// </summary>
    private int livesCache;

    void Start()
    {
        // Get gameManager
        gameManager = GameManager.Instance;

        // Get elements
        VisualElement root = UIDoc.rootVisualElement;
        livesContainer = root.Q<VisualElement>("Lives");
        timerLabel = root.Q<Label>("TimerValue");

        // Set initial timer
        timerLabel.text = "" + levelTimer.Duration;

        // Initial lives
        for (int i = 0; i < gameManager.Lives; i++)
        {
            // Add initial life
            addLife();
        }

        // Cache initial lives
        livesCache = gameManager.Lives;

        // Listen for lives changes
        gameManager.OnLivesChanged.AddListener(UpdateLives);
    }
    void OnDestroy()
    {
        // Remove lives listener
        gameManager.OnLivesChanged.RemoveListener(UpdateLives);

        // Kill timer coroutine
        levelTimer.ResetTimer();
    }

    /// <summary>
    /// Adds a life to lives container
    /// </summary>
    void addLife()
    {
        // Set life image
        Image life = new Image();
        life.sprite = lifeImage;

        // Set padding
        life.style.paddingTop = 5;
        life.style.paddingLeft = 0;
        life.style.paddingRight = 0;

        // Set size
        life.style.width = 16;
        life.style.height = 16;

        // Add to container
        livesContainer.Add(life);
    }

    /// <summary>
    /// Removes a life from lives container
    /// </summary>
    void removeLife()
    {
        // remove life
        livesContainer.RemoveAt(0);
    }

    /// <summary>
    /// Updates the lives counter 
    /// </summary>
    void UpdateLives()
    {
        // Get new lives count
        int newLivesCount = gameManager.Lives;

        // Compare to previous lives count
        if (newLivesCount > livesCache)
        {
            // Add additional lives
            for (int i = 0; i < Mathf.Abs(newLivesCount - livesCache); i++)
            {
                addLife();
            }
        }
        else
        {
            // Remove lost lives
            for (int i = 0; i < Mathf.Abs(newLivesCount - livesCache); i++)
            {
                removeLife();
            }
        }

        // Update lives count cache
        livesCache = newLivesCount;
    }

    /// <summary>
    /// Called when level timer expires
    /// </summary>
    public void LevelTimerExpired()
    {
        // Game over
        gameManager.GameOver();
    }
}
