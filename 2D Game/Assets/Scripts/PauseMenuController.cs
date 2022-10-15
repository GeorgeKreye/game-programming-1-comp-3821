using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controls a pause menu UI
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    [Tooltip("The UI Document used by the pause menu")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The Player GameObject")]
    [SerializeField] private GameObject player;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    // Buttons
    /// <summary>
    /// The button used to resume the game
    /// </summary>
    private Button resumeButton;
    /// <summary>
    /// The button used to return to main menu
    /// </summary>
    private Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        // Hide UI
        OnResumed();

        // Get GameManager
        gameManager = GameManager.Instance;

        // Get buttons
        VisualElement root = UIDoc.rootVisualElement;
        resumeButton = root.Q<Button>("ResumeButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");

        // Assign actions to buttons
        resumeButton.clicked += ResumeButtonClicked;
        mainMenuButton.clicked += MainMenuButtonClicked;

        // Listen for pausing/unpausing
        gameManager.OnGamePaused.AddListener(OnPaused);
        gameManager.OnGameResumed.AddListener(OnResumed);
    }

    // OnDestroy is called before a script instance is destroyed
    private void OnDestroy()
    {
        // Remove assigned actions from buttons
        resumeButton.clicked -= ResumeButtonClicked;
        mainMenuButton.clicked -= MainMenuButtonClicked;

        // Stop listening
        gameManager.OnGamePaused.RemoveListener(OnPaused);
        gameManager.OnGameResumed.RemoveListener(OnResumed);
    }

    /// <summary>
    /// Called when the resume button is clicked
    /// </summary>
    void ResumeButtonClicked()
    {
        // Resume game
        gameManager.ResumeGame();
    }

    /// <summary>
    /// Called when the main menu button is clicked
    /// </summary>
    void MainMenuButtonClicked()
    {
        // Go to main menu
        gameManager.ChangeScene("MainMenu");
    }

    /// <summary>
    /// Called when game is paused
    /// </summary>
    void OnPaused()
    {
        // Show pause menu
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }

    /// <summary>
    /// Called when game is resumed
    /// </summary>
    void OnResumed()
    {
        // Hide pause menu
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;
    }
}
