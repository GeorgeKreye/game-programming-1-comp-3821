using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action for attacking the player
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Attack",
    fileName = "Attack")]
public class AttackAction : Action
{
    [Tooltip("The amount of damage attacking does")]
    [SerializeField] private int damage = 1;

    // Performs action
    public override void Act(AIStateController controller)
    {
        // Call internal method
        Attack(controller);
    }

    /// <summary>
    /// Inflicts damage to player and plays attack animation
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void Attack(AIStateController controller)
    {
        // Tell game manager to inflict damage
        controller.gameManager.RemoveHealth(damage);

        // Play attack animation
        controller.animator.SetTrigger("Attack");

        // Tell controller that AI has attacked at least once
        if (!controller.hasAttacked)
        {
            controller.hasAttacked = true;
        }
    }
}
