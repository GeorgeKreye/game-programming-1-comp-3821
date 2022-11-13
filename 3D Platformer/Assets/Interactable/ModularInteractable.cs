using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for performing interactable behavior using scriptable objects
/// </summary>
public class ModularInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("The interactable scriptable object to use")]
    [SerializeField] private InteractableSO interactBehavior;

    // Perform interaction
    public void Interact(CharacterInteractManager manager,
        CharacterController controller)
    {
        interactBehavior.InteractBehavior(manager, controller);
    }
}
