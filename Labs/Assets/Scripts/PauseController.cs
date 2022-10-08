using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : MonoBehaviour
{
    // Fields
    [Tooltip("The UI Document used as the pause menu")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The Player Game Object to 'unfreeze' if resume button is " +
        "pressed")]
    [SerializeField] private GameObject player;
    [Tooltip("The HUD UI containing the game timer to pause/unpause")]
    [SerializeField] private GameObject HUD;

    // Buttons
    private Button resumeButton;
    private Button quitButton;

    // Player control script
    private PausePlayerControl playerControl;

    // HUD timer
    private Timer HUDTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Get HUD timer compnent
        HUDTimer = HUD.GetComponent<Timer>();
        if (HUDTimer == null)
        {
            Debug.LogError("Could not find a Timer script on HUD object");
        }

        // Hide pause UI
        OnPlayerUnpaused();

        // Get Player Control component
        playerControl = player.GetComponent<PausePlayerControl>();
        if (playerControl == null)
        {
            Debug.LogError("Could not find a PausePlayerControl script on " +
                "player object");
        }

        // Get buttons
        VisualElement root = UIDoc.rootVisualElement;
        resumeButton = root.Q<Button>("ResumeButton");
        quitButton = root.Q<Button>("QuitButton");

        // Set button functions
        resumeButton.clicked += ResumeButtonClicked;
        quitButton.clicked += QuitButtonClicked;
    }

    public void OnPlayerPaused()
    {
        // Make pause menu visible
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;

        // Pause HUD timer
        HUDTimer.PauseTimer();
    }
    public void OnPlayerUnpaused()
    {
        // Make pause menu invisible
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;

        // Resume HUD timer
        HUDTimer.ResumeTimer();
    }

    void ResumeButtonClicked()
    {
        // Unfreeze player
        playerControl.SwitchCurrentActionMap("Player");

        // Hide pause menu
        OnPlayerUnpaused();
    }

    void QuitButtonClicked()
    {
        Debug.Log("No Main Menu implemented, full quitting instead");
        Application.Quit();
    }
}
