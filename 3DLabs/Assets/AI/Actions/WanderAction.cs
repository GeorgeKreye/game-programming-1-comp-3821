using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AI action for wandering.
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Actions/Wander", fileName =
    "WanderA")]
public class WanderAction : Action
{
    [Tooltip("The maximum distance from a point to check the NavMesh for a " +
        "valid position")]
    [SerializeField] private float sampleRange = 10f;

    /// <summary>
    /// Target position for current wander
    /// </summary>
    private Vector3 target;

    /// <summary>
    /// Whether the current iteration of Act is the first
    /// </summary>
    private bool init = true;

    // Performs action
    public override void Act(AIStateController controller)
    {
        // Update target position if at target positon
        if (controller.transform.position.Equals(target) || init)
        {
            UpdateTarget(controller);
            
        }

        // Move towards target position
        MoveToTarget(controller);

        // Turn off init if on
        if (init)
        {
            init = false;
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
        NavMesh.SamplePosition(newTarget, out hit, sampleRange, 0);

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
