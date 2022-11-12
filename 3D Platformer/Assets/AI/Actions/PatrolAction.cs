using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI action for patrolling between a set of points
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Patrol", fileName = "Patrol")]
public class PatrolAction : Action
{
    #region Pausing fields
    /// <summary>
    /// The time that has passed since reaching a waypoint and pausing
    /// </summary>
    private float pauseTime = -1; // -1 means no active pause

    /// <summary>
    /// The time on the controller timer that pausing started
    /// </summary>
    private float pauseStart;
    #endregion

    // Perform action
    public override void Act(AIStateController controller)
    {
        // Call internal function
        Patrol(controller);
    }

    /// <summary>
    /// Has the AI move to the next patrol point, updating it and waiting as
    /// needed
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void Patrol(AIStateController controller)
    {
        // Check if currently at a waypoint
        if (controller.transform.position.Equals(
            controller.patrolWaypoints[controller.currentPWaypoint].position))
        {
            // Increment current waypoint
            controller.currentPWaypoint = (controller.currentPWaypoint + 1) %
                controller.patrolWaypoints.Length;

            // Stop agent
            controller.agent.isStopped = true;

            // Pause
            pauseTime = 0f;

            // Get pausing start time
            pauseStart = controller.timer;
        }

        // Wait if paused
        if (pauseTime > -1)
        {
            // Increment pause time
            pauseTime = controller.timer - pauseStart;

            // Check if pause is over
            if (pauseTime >= controller.patrolPauseDuration)
            {
                // End pause
                pauseTime = -1;
            }
        }
        else
        {
            // Move to next waypoint if not paused
            controller.agent.SetDestination(
                controller.patrolWaypoints[
                    controller.currentPWaypoint].position);
        }
    }
}
