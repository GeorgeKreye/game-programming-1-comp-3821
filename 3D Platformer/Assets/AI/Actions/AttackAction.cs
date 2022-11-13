using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action for attacking a target (usually the player)
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Attack",
    fileName = "Attack")]
public class AttackAction : Action
{
    [Tooltip("Layer mask containing attackable objects (should only contain" +
        "objects that can be damaged")]
    [SerializeField] private LayerMask attackableLayerMask;

    // Performs action
    public override void Act(AIStateController controller)
    {
        // Call internal method
        Attack(controller);
    }

    /// <summary>
    /// Attacks a target, given its attackable
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void Attack(AIStateController controller)
    {
        // Make attack cast
        RaycastHit hit;
        if (Physics.SphereCast(controller.AIEyes.position,
            controller.attackRadius, controller.AIEyes.forward, out hit,
            controller.attackRange, attackableLayerMask))
        {
            // Perform attack animation
            controller.animator.SetTrigger("Attack");
        }
    }
}
