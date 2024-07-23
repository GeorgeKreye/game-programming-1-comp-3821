using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision that determines whether the AI is at its home position
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/At Home",
    fileName = "AtHome")]
public class AtHomeDecision : Decision
{

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // Return result of internal method
        return AtHome(controller);
    }

    /// <summary>
    /// Checks if the AI's current position (approximately) matches its home
    /// positon
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this decision
    /// </param>
    /// <returns>
    /// Whether the current position matches the home position
    /// </returns>
    private bool AtHome(AIStateController controller)
    {
        // Calculate horizontal distance
        float horizontalDistance = Vector3.Distance(new Vector3(
            controller.transform.position.x, controller.homeWaypoint.position.y,
            controller.transform.position.z), controller.homeWaypoint.position);

        // Use Mathf.Approximtately if threshold is 0
        if (controller.threshold == 0)
        {
            return Mathf.Approximately(horizontalDistance, 0f);
        }

        // Otherwise compare to threshold directly
        return horizontalDistance < controller.threshold;
    }
}
