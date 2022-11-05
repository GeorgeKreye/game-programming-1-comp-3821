using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object abstract class for AI states
/// </summary>
public abstract class AIState : ScriptableObject
{
    /// <summary>
    /// Perform action(s) associated with this AI state
    /// </summary>
    /// <param name="controller">The AI Controller calling Act</param>
    public abstract void Act(AIStateController controller);
}
