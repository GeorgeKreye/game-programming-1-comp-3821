using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object abstract class for AI states
/// </summary>
public class AIState : ScriptableObject
{
    [Header("Actions")]
    [Tooltip("The list of actions that occur when entering this state")]
    [SerializeField] private List<Action> enterActions;
    [Tooltip("The list of actions that occur when exiting this state")]
    [SerializeField] private List<Action> exitActions;
    [Tooltip("The list of actions that occur when updating this state")]
    [SerializeField] private List<Action> updateActions;

    [Header("Transitions")]
    [Tooltip("The list of all possible transitions from this state")]
    [SerializeField] private List<Transition> transitions;

    /// <summary>
    /// Function to be called when entering this state
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    public void EnterState(AIStateController controller)
    {
        // Perform entrance actions
        DoEnterActions(controller);
    }

    /// <summary>
    /// Function to be called when exiting this state
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    public void ExitState(AIStateController controller)
    {
        // Perform exit actions
        DoExitActions(controller);
    }

    /// <summary>
    /// Function to be called when updating this state
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    public void UpdateState(AIStateController controller)
    {
        // Perform update actions
        DoUpdateActions(controller);
    }

    private void DoEnterActions(AIStateController controller)
    {
        foreach (Action enterAction in enterActions)
        {

        }
    }

    private void DoExitActions(AIStateController controller)
    {
        foreach (Action exitAction in exitActions)
        {

        }
    }

    private void DoUpdateActions(AIStateController controller)
    {
        foreach (Action updateAction in updateActions)
        {

        }
    }
}
