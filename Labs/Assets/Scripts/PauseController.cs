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

    // Buttons
    private Button resumeButton;
    private Button quitButton;

    // Player control script
    private PausePlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        // Hide pause UI
        OnPlayerUnpaused();

        // Get Player Input component
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
    }

    public void OnPlayerPaused()
    {
        // Make pause menu visible
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }
    public void OnPlayerUnpaused()
    {
        // Make pause menu invisible
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;
    }

    void ResumeButtonClicked()
    {
        // Unfreeze player
        playerControl.SwitchCurrentActionMap("Player");

        // Hide pause menu
        OnPlayerUnpaused();
    }
}
