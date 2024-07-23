using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scriptable Object abstract for interactables
/// </summary>
public abstract class InteractableSO : ScriptableObject
{
    /// <summary>
    /// Behavior to perform on interaction
    /// </summary>
    /// <param name="manager">
    /// The character interact manager referencing this interactable SO
    /// </param>
    /// <param name="controller">
    /// The character controller referencing this interactable SO
    /// </param>
    public abstract void InteractBehavior(CharacterInteractManager manager,
        CharacterController controller);
}
