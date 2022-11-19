using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision for if end of patrol has been reached
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/EndOfPatrol",
    fileName = "EndOfPatrol")]
public class EndOfPatrolDecision : Decision
{
    [Tooltip("The maximum number of patrol cycles that can occur before this " +
        "decision can resolve to true")]
    [SerializeField] private int maxCycles;

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // Return if current waypoint is last one and last cycle has occured
        return controller.currentPWaypoint ==
            controller.patrolWaypoints.Length - 1 && controller.patrolCycles >=
            maxCycles;
    }
}
