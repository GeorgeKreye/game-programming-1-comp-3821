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
        // Perform logic
        return Mathf.Approximately(Vector3.Distance(
            controller.homeWaypoint.position, controller.transform.position),
            0f);
    }
}
