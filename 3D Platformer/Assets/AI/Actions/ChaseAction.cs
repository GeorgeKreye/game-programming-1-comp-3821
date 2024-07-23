using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI action for chasing after a target (usually the player)
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Chase", fileName = "Chase")]
public class ChaseAction : Action
{
    // Perform action
    public override void Act(AIStateController controller)
    {
        // Call internal method
        Chase(controller);
    }

    /// <summary>
    /// Chases after a target GameObject
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void Chase(AIStateController controller)
    {
        // Set target & speed and begin movement
        controller.agent.SetDestination(controller.chaseTarget.position);
        controller.agent.speed = controller.runSpeed;
        controller.agent.isStopped = false;

        // Tell controller agent is running
        controller.isRunning = true;
    }
}
