using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controls the main menu UI
/// </summary>
public class MainMenuControl : MonoBehaviour
{
    // fields
    [Tooltip("The UI Document used by this GameObject")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The name of the starting scene of the game")]
    [SerializeField] private string playScene;
    [Tooltip("The Credits UI object")]
    [SerializeField] private GameObject credits;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// The script controlling the credits UI
    /// </summary>
    private CreditsController creditsControl;

    //buttons
    /// <summary>
    /// The play button of the UI
    /// </summary>
    private Button playButton;
    /// <summary>
    /// The credits button of the UI
    /// </summary>
    private Button creditsButton;
    /// <summary>
    /// The quit button of the UI
    /// </summary>
    private Button quitButton;

    void Awake()
    {
        // Get buttons
        VisualElement root = UIDoc.rootVisualElement;
        playButton = root.Q<Button>("PlayButton");
        creditsButton = root.Q<Button>("CreditsButton");
        quitButton = root.Q<Button>("QuitButton");

        // Set button behavior
        playButton.clicked += PlayButtonClicked;
        creditsButton.clicked += CreditsButtonClicked;
        quitButton.clicked += QuitButtonClicked;
    }

    void Start()
    {
        // Get game manager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("Could not find an active GameManager instance");
        }

        // Get credits controller
        creditsControl = credits.GetComponent<CreditsController>();
        if (creditsControl == null)
        {
            Debug.LogError("Could not get controller for menu credits UI");
        }
    }

    void OnDestroy()
    {
        // Remove button behavior
        playButton.clicked -= PlayButtonClicked;
        creditsButton.clicked -= CreditsButtonClicked;
        quitButton.clicked -= QuitButtonClicked;
    }

    /// <summary>
    /// Changes scene upon the play button being clicked
    /// </summary>
    void PlayButtonClicked()
    {
        // Tell game manager to switch scene
        gameManager.ChangeScene(playScene);
    }

    /// <summary>
    /// Opens menu credits upon the credits button being clicked
    /// </summary>
    void CreditsButtonClicked()
    {
        // Hide this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;

        // Open credits
        creditsControl.Open();
    }

    /// <summary>
    /// Exits game upon the quit button being clicked
    /// </summary>
    void QuitButtonClicked()
    {
        // Exit game
        Application.Quit();
    }

    /// <summary>
    /// Opens the main menu from the credits menu
    /// </summary>
    public void Open()
    {
        // Show this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }
}
