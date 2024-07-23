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

    [Tooltip("Whether this interaction can only be used once")]
    [SerializeField] private bool oneUse = false;

    // Perform interaction
    public void Interact(CharacterInteractManager manager,
        CharacterController controller)
    {
        interactBehavior.InteractBehavior(manager, controller);

        // Destroy if one use
        if (oneUse)
        {
            manager.StopTrackingObject(gameObject);
            Destroy(this);
        }
    }
}
