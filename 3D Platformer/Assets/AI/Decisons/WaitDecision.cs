using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision that waits for a specified period of time before returning true
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/Wait", fileName = "Wait")]
public class WaitDecision : Decision
{
    [Tooltip("The amount of time to wait for")]
    [SerializeField] private float duration;

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // Return whether timer duration is reached
        return controller.timer >= duration;
    }
}
