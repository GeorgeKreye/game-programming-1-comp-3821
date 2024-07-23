using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable SO for performing multiple interaction behaviors at once
/// </summary>
[CreateAssetMenu(menuName = ("Interactables/Composite"), fileName =
    "CompositeISO")]
public class CompositeInteractableSO : InteractableSO
{
    [Tooltip("The Interactable SOs used in this composite interactable")]
    [SerializeField] private InteractableSO[] interactables;

    // Performs interact behaviors
    public override void InteractBehavior(CharacterInteractManager manager,
        CharacterController controller)
    {
        // Loop through interactable SOs
        foreach (InteractableSO interactable in interactables)
        {
            // Perform behaviors
            interactable.InteractBehavior(manager, controller);
        }
    }
}
