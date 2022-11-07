using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision that determines if the AI can see the player
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/Look", fileName = "Look")]
public class LookDecision : Decision
{
    [Tooltip("The layer the player character is on")]
    [SerializeField] private LayerMask characterLayerMask;

    public override bool Decide(AIStateController controller)
    {
        // Return whether the target is visible
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    /// <summary>
    /// Checks whether an AI can see the player
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this decision
    /// </param>
    /// <returns>
    /// Whether the AI can see the player object
    /// </returns>
    private bool Look(AIStateController controller)
    {
        RaycastHit hit;
        Collider[] colliders;
        if (Physics.SphereCast(controller.AIEyes.position,
            controller.lookRadius, controller.AIEyes.forward, out hit,
            controller.lookRange, characterLayerMask,
            QueryTriggerInteraction.Ignore))
        {

        }
    }
}
