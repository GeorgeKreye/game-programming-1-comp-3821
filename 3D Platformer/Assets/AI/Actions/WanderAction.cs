using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AI action for wandering.
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Wander", fileName =
    "Wander")]
public class WanderAction : Action
{
    /// <summary>
    /// Target position for current wander
    /// </summary>
    private Vector3 target;

    // Performs action
    public override void Act(AIStateController controller)
    {
        // Update target position if at target positon or (re)starting
        if (controller.transform.position.Equals(target) ||
            controller.wanderRestart)
        {
            // Update target
            UpdateTarget(controller);

            // Pause if not (re)starting
            if (!controller.wanderRestart)
            {

            }
        }

        // Move towards target position
        MoveToTarget(controller);

        // Turn off restart if on
        if (controller.wanderRestart)
        {
            controller.wanderRestart = false;
        }
    }

    #region Internal functions
    /// <summary>
    /// Updates the target position of movement.
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void UpdateTarget(AIStateController controller)
    {
        // Get center
        Vector3 center = controller.homeWaypoint.position;

        // Calculate 
        Vector2 temp = Random.insideUnitCircle * controller.wanderRadius;
        Vector3 newTarget = new Vector3(center.x + temp.x, center.y, center.z +
            temp.y);

        // Sample nav mesh to find target
        NavMeshHit hit;
        NavMesh.SamplePosition(newTarget, out hit, controller.checkRadius, 0);

        // Set target
        target = hit.position;
    }

    /// <summary>
    /// Moves towards the current target position
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void MoveToTarget(AIStateController controller)
    {
        // Set destination and begin movement
        controller.agent.SetDestination(target);
        controller.agent.isStopped = false;
    }
    #endregion
}
