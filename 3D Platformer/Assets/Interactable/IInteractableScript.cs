using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for scripts that respond to interaction
/// </summary>
public interface IInteractableScript
{
    /// <summary>
    /// Script behavior to perform on interaction
    /// </summary>
    /// <param name="manager">
    /// The character interaction manager referencing this script
    /// </param>
    /// <param name="controller">
    /// The character controller referencing this script
    /// </param>
    public void OnInteract(CharacterInteractManager manager,
        CharacterController controller);
}
