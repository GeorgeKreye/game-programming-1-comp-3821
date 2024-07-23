using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision used to determine if the AI is able to attack
/// </summary>
[CreateAssetMenu (menuName = "Pluggable AI/Decisions/Attack",
    fileName = "Attack")]
public class AttackDecision : Decision
{
    [Tooltip("The layer the player is on")]
    [SerializeField] private LayerMask targetLayerMask;

    // Perform decision logic
    public override bool Decide(AIStateController controller)
    {
        // Call internal method
        return Attack(controller);
    }

    /// <summary>
    /// Checks whether an AI can hit the player using a sphere cast
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this decision
    /// </param>
    /// <returns>
    /// Whether the attack cast hits
    /// </returns>
    private bool Attack(AIStateController controller)
    {
        // Make attack cast
        RaycastHit hit;
        if (Physics.SphereCast(controller.AIEyes.position,
            controller.attackRadius, controller.AIEyes.forward, out hit,
            controller.attackRange, targetLayerMask,
            QueryTriggerInteraction.Ignore))
        {
            // Set as target and return hit
            controller.chaseTarget = hit.transform;
            return true;
        }
        return false; // No cast hit
    }
}
