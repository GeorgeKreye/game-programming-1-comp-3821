using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for an interactable button. Used in conjuction with a
/// ModularInteractable instance which uses a CallScript ISO.
/// </summary>
public class PressableButton : MonoBehaviour, IInteractableScript
{
    [Tooltip("The Animator used by this GameObject")]
    [SerializeField] private Animator animator;
    [Tooltip("Whether this GameObject can only be interacted with once")]
    [SerializeField] private bool oneUse = false;

    [Tooltip("Event invoked on the button being interacted with")]
    public UnityEvent OnButtonPressed;

    /// <summary>
    /// Whether a one-use interaction has been used
    /// </summary>
    private bool used = false;

    // Called on interact
    public void OnInteract(CharacterInteractManager manager,
        CharacterController controller)
    {
        // Only perform behavior if not used
        if (!used)
        {
            // Invoke button press event
            OnButtonPressed.Invoke();

            // Animate if animator is not null
            if (animator != null)
            {
                animator.SetTrigger("Pressed");
            }

            // Stop further interaction
            if (oneUse)
            {
                used = true;
            }
        }
    }
}
