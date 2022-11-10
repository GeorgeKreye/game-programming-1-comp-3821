using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object abstract for AI decision making
/// </summary>
public abstract class Decision : ScriptableObject
{
    /// <summary>
    /// Performs the decision-making logic associated with this decision
    /// </summary>
    /// <param name="controller">
    /// The AI State Controller referencing this decision
    /// </param>
    /// <returns>
    /// The boolean result of the decision
    /// </returns>
    public abstract bool Decide(AIStateController controller);
}
