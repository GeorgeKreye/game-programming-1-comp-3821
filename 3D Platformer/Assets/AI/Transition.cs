using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for AI state transitions
/// </summary>
[System.Serializable]
public class Transition
{
    [Tooltip("The decision being evaluated by this transition")]
    public Decision decision;

    [Tooltip("The next state to transition to")]
    public AIState nextState;
}
