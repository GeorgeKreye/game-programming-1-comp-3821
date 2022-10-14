using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuControl : MonoBehaviour
{
    // fields
    [Tooltip("The UI Document used by this GameObject")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The name of the starting scene of the game")]
    [SerializeField] private string playScene;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    //buttons
    /// <summary>
    /// The play button of the UI
    /// </summary>
    private Button playButton;
    /// <summary>
    /// The quit button of the UI
    /// </summary>
    private Button quitButton;

    void Awake()
    {
        // Get buttons
        VisualElement root = UIDoc.rootVisualElement;
        playButton = root.Q<Button>("PlayButton");
        quitButton = root.Q<Button>("QuitButton");

        // Set button behavior
        playButton.clicked += PlayButtonClicked;
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

        // Tell game manager that we are on main menu
        gameManager.MenuActive();
    }

    void OnDestroy()
    {
        // Remove button behavior
        playButton.clicked -= PlayButtonClicked;
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
    /// Exits game upon the quit button being clicked
    /// </summary>
    void QuitButtonClicked()
    {
        // Exit game
        Application.Quit();
    }
}
