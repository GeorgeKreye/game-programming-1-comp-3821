using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausePlayerControl : MonoBehaviour
{
    [Tooltip("The PlayerInput component to use")]
    [SerializeField] private PlayerInput input;

    [Tooltip("The Pause UI to send pause state to")]
    [SerializeField] private GameObject pauseUI;

    [Tooltip("The HUD of the game")]
    [SerializeField] private GameObject HUD;

    // The controller for the Pause UI
    private PauseController controller;

    // The controller for the HUD
    private HUDController HUDcontroller;

    // Start is called before the first frame update
    void Start()
    {
        controller = pauseUI.GetComponent<PauseController>();
        if (controller == null)
        {
            Debug.LogError("Could not find a PauseController attached to " +
                "Pause UI");
        }
        HUDcontroller = HUD.GetComponent<HUDController>();
        if (HUDcontroller == null)
        {
            Debug.LogError("Could not find a HUDController attached to " +
                "game HUD");
        }
    }

    // Pause
    public void OnPlayerPause(InputAction.CallbackContext context)
    {
        SwitchCurrentActionMap("UI");
        controller.OnPlayerPaused();
    }

    // Unpause
    public void OnPlayerUnpause(InputAction.CallbackContext context)
    {
        SwitchCurrentActionMap("Player");
        controller.OnPlayerUnpaused();
    }

    public void SwitchCurrentActionMap(string mapName)
    {
        // Disable current action map
        input.currentActionMap.Disable();

        // Switch to new action map
        input.SwitchCurrentActionMap(mapName);

        // Change mouse state
        switch (mapName)
        {
            case "UI":
                // Unlock cursor and show mouse
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                // Lock cursor and hide mouse
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    // Heart adding/removing testing

    public void HeartAdd(InputAction.CallbackContext context)
    {
        // Add heart
        HUDcontroller.AddLife();
        Debug.Log("Added heart");
    }

    public void HeartRemove(InputAction.CallbackContext context)
    {
        // Remove heart
        HUDcontroller.RemoveLife();
        Debug.Log("Removed heart");
    }
}
