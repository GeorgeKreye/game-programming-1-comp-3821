using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision that determines if the AI can see a target GameObject (usually
/// the player)
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/Look", fileName = "Look")]
public class LookDecision : Decision
{
    [Tooltip("The layer the target object (usually the player) is on")]
    [SerializeField] private LayerMask targetLayerMask;

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // Return whether the target is visible
        return Look(controller);
    }

    /// <summary>
    /// Checks whether an AI can see a target object (usually the player) using
    /// a sphere cast
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this decision
    /// </param>
    /// <returns>
    /// Whether the AI can see a target object
    /// </returns>
    private bool Look(AIStateController controller)
    {
        // Make cast
        RaycastHit hit;
        if (Physics.SphereCast(controller.AIEyes.position,
            controller.lookRadius, controller.AIEyes.forward, out hit,
            controller.lookRange, targetLayerMask,
            QueryTriggerInteraction.Ignore))
        {
            // Set as target and return hit
            controller.chaseTarget = hit.transform;
            Debug.Log("Found target");
            return true;
        }
        return false; // No cast hit
    }
}
