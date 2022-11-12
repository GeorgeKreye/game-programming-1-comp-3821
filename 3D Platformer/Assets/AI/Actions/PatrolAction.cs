using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI action for patrolling between a set of points.
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Patrol", fileName = "Patrol")]
public class PatrolAction : Action
{
    // Perform action
    public override void Act(AIStateController controller)
    {
        // Call internal function
        Patrol(controller);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this action
    /// </param>
    private void Patrol(AIStateController controller)
    {

    }
}
