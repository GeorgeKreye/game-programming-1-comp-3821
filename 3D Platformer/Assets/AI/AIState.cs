using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object class for AI states
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/AI State", fileName = "AIState")]
public class AIState : ScriptableObject
{
    #region Serialized Fields
    [Header("Actions")]
    #region Action Lists
    [Tooltip("The list of actions that occur when entering this state")]
    [SerializeField] private List<Action> enterActions;
    [Tooltip("The list of actions that occur when exiting this state")]
    [SerializeField] private List<Action> exitActions;
    [Tooltip("The list of actions that occur when updating this state")]
    [SerializeField] private List<Action> updateActions;
    #endregion

    [Header("Transitions")]
    [Tooltip("The list of all possible transitions from this state")]
    [SerializeField] private List<Transition> transitions;
    #endregion

    #region Public Functions
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

        // Check for any transitions to make
        CheckTransitions(controller);
    }
    #endregion

    #region Actions
    /// <summary>
    /// Performs all actions listed in enterActions
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    private void DoEnterActions(AIStateController controller)
    {
        // Perform all actions in enterActions
        foreach (Action enterAction in enterActions)
        {
            enterAction.Act(controller);
        }
    }

    /// <summary>
    /// Performs all actions listed in exitActions
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    private void DoExitActions(AIStateController controller)
    {
        // Perform all actions in exitActions
        foreach (Action exitAction in exitActions)
        {
            exitAction.Act(controller);
        }
    }

    /// <summary>
    /// Performs all actions listed in updateActions
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    private void DoUpdateActions(AIStateController controller)
    {
        // Perform all actions in updateActions
        foreach (Action updateAction in updateActions)
        {
            updateAction.Act(controller);
        }
    }
    #endregion

    /// <summary>
    /// Checks whether a transition should be performed
    /// </summary>
    /// <param name="controller">
    /// The AI state controller referencing this AI state
    /// </param>
    private void CheckTransitions(AIStateController controller)
    {
        // Check if any transitions are to be made
        foreach (Transition transition in transitions)
        {
            // Determine if transition is to be made
            bool decision = transition.decision.Decide(controller);

            // Make transition if it is to be made
            if (decision)
            {
                // Go to next state
                controller.TransitionToState(transition.nextState);
                break; // Stop looping through transitions
            }
        }
    }
}
