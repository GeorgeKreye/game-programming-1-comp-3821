using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action to move a Navigation Agent towards a 'home' waypoint
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Go Home",
    fileName = "GoHome")]
public class GoHomeAction : Action
{
    // Performs action
    public override void Act(AIStateController controller)
    {
        // Call internal function
        GoHome(controller);
    }

    /// <summary>
    /// Has an AI move towards the 'home' waypoint
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void GoHome(AIStateController controller)
    {
        // Set destination and begin movement
        controller.agent.SetDestination(controller.homeWaypoint.position);
        controller.agent.isStopped = false;
    }
}
