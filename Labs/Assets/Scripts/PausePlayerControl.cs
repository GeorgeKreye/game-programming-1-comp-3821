using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausePlayerControl : MonoBehaviour
{
    [Tooltip("The PlayerInput component to use")]
    [SerializeField] private PlayerInput input;

    // Pause
    public void OnPlayerPause(InputAction.CallbackContext context)
    {
        SwitchCurrentActionMap("UI");
    }

    // Unpause
    public void OnPlayerUnpause(InputAction.CallbackContext context)
    {
        SwitchCurrentActionMap("Player");
    }

    private void SwitchCurrentActionMap(string mapName)
    {
        // Disable current action map
        input.currentActionMap.Disable();

        // Switch to new action map
        input.SwitchCurrentActionMap(mapName);

        // Change mouse state
        switch(mapName)
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
}
