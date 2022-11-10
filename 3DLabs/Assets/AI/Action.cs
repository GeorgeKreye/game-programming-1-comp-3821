using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object abstract for AI actions
/// </summary>
public abstract class Action : ScriptableObject
{
    /// <summary>
    /// Performs this action
    /// </summary>
    /// <param name="controller">
    /// The AI State Controller referencing this action
    /// </param>
    public abstract void Act(AIStateController controller);
}
