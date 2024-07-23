using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

/// <summary>
/// Controls the win menu UI
/// </summary>
public class WinMenuController : MonoBehaviour
{
    #region Component/Object references
    [Tooltip("The UI Document used by the win menu")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The credits UI object")]
    [SerializeField] private GameObject creditsUI;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// The script controlling the credits UI
    /// </summary>
    private WinCreditsControl creditsControl;
    #endregion

    #region Button pointers
    /// <summary>
    /// The win UI button used to display credits
    /// </summary>
    private Button creditsButton;

    /// <summary>
    /// The win UI button used to go to main menu
    /// </summary>
    private Button menuButton;

    /// <summary>
    /// The win UI button used to quit the game
    /// </summary>
    private Button quitButton;
    #endregion

    #region Unity Functions
    // Awake is called when a script instance is created
    void Awake()
    {
        // Get UI doc root
        VisualElement root = UIDoc.rootVisualElement;

        // Get buttons
        creditsButton = root.Q<Button>("creditsButton");
        menuButton = root.Q<Button>("menuButton");
        quitButton = root.Q<Button>("quitButton");

        // Subscribe to button presses
        SubscribeButtonActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get game manager instance
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("Could not find GameManager instance");
        }

        // Get credits UI
#pragma warning disable UNT0026 // Handled by following if statement
        creditsControl = creditsUI.GetComponent<WinCreditsControl>();
#pragma warning restore UNT0026 
        if (creditsControl == null)
        {
            Debug.LogError("Could not find a controller on the provided " +
                "credits UI");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnDestroy is called when a script instance is to be removed
    void OnDestroy()
    {
        // Unsubscribe from button presses
        UnsubscribeButtonActions();
    }
    #endregion

    #region Button listening
    #region Event subscription
    /// <summary>
    /// Subscribes listening methods to button events
    /// </summary>
    private void SubscribeButtonActions()
    {
        creditsButton.clicked += CreditsButtonClicked;
        menuButton.clicked += MenuButtonClicked;
        quitButton.clicked += QuitButtonClicked;
    }

    /// <summary>
    /// Unsubscribes listening methods from button events
    /// </summary>
    private void UnsubscribeButtonActions()
    {
        creditsButton.clicked += CreditsButtonClicked;
        menuButton.clicked += MenuButtonClicked;
        quitButton.clicked += QuitButtonClicked;
    }
    #endregion

    #region Event listening
    /// <summary>
    /// Opens credits upon credits button being clicked
    /// </summary>
    private void CreditsButtonClicked()
    {
        // Close this UI
        Close();

        // Open credits UI
        creditsControl.Open();
    }

    /// <summary>
    /// Goes to menu upon menu button being clicked
    /// </summary>
    private void MenuButtonClicked()
    {
        // Go to menu
        gameManager.ChangeScene("Menu");
    }

    /// <summary>
    /// Quits the game upon quit button being clicked
    /// </summary>
    private void QuitButtonClicked()
    {
        // Quit game
        Application.Quit();
    }
    #endregion
    #endregion

    #region UI opening and closing
    /// <summary>
    /// Opens this UI
    /// </summary>
    public void Open()
    {
        // Show this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }

    /// <summary>
    /// Closes this UI
    /// </summary>
    void Close()
    {
        // Hide this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;
    }
    #endregion
}
