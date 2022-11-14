using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calls the OnInteract function(s) of (an) InteractableScript instance(s)
/// attached to the GameObject being interacted with
/// </summary>
[CreateAssetMenu(menuName = "Interactables/Call Scripts",
    fileName = "CallScriptsISO")]
public class CallScriptsInteractableSO : InteractableSO
{
    // Perform interaction behavior
    public override void InteractBehavior(CharacterInteractManager manager,
        CharacterController controller)
    {
        // Get interactable scripts
        IInteractableScript[] scripts =
            manager.GetActiveInteractable().GetComponents<IInteractableScript>();

        // Call OnInteract methods of scripts
        foreach(IInteractableScript script in scripts)
        {
            script.OnInteract(manager, controller);
        }
    }
}
