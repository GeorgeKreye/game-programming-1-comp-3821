using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interface that defines an interactable
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Performs the interaction behavior of this Interactable
    /// </summary>
    /// <param name="manager">
    /// The character interact manager referencing this interactable
    /// </param>
    /// <param name="controller">
    /// The character controller referencing this interactable
    /// </param>
    void Interact(CharacterInteractManager manager,
        CharacterController controller);
}
