using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for an AI Agent to handle AI states
/// </summary>
public class AIStateController : MonoBehaviour
{
    [Header("State Control")]
    [Tooltip("The current state the FSM is in")]
    public AIState currentState;
    [Tooltip("Whehter this FSM is active")]
    public bool isActive;

    [Header("Blackboard")]
    [Tooltip("The NavMeshAgent used by this controller")]
    public NavMeshAgent agent;
    [Tooltip("The position to use as the 'home' waypoint")]
    public Transform homeWaypoint;
    [Tooltip("Position to use as eye level")]
    public Transform AIEyes;
    [Tooltip("Maximum visual distance")]
    public float lookRadius;
    [Tooltip("Maximum visual range")]
    public float lookRange;
    [Tooltip("Target object for tracking (usualy the player)")]
    public Transform chaseTarget;
    [Tooltip("Maximum radius when wandering")]
    public float wanderRadius;
    [Tooltip("Timer for waiting")]
    public float timer = 0f;


    #region Unity Functions
    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Set up AI
        Setup();
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // call update actions
        if (isActive)
        {
            // Update current state
            currentState.UpdateState(this);
        }

        // update timer
        timer += Time.deltaTime;
    }
    #endregion

    /// <summary>
    /// Sets up the FSM
    /// </summary>
    public void Setup()
    {
        // Make sure home waypoint is on the navigation mesh
        NavMeshHit hit;
        NavMesh.SamplePosition(homeWaypoint.position,out hit, 10f,0);
    }

    /// <summary>
    /// Changes the active AI state to the specified one.
    /// </summary>
    /// <param name="newState">
    /// The new AI State to use
    /// </param>
    public void TransitionToState(AIState newState)
    {
        // Make sure new state is not the same as current state
        if (currentState != newState)
        {
            // Perform exit actions of old state
            currentState.ExitState(this);

            // Change current state to new state
            currentState = newState;

            // Reset timer
            timer = 0f;

            // Perform enter actions of new state
            currentState.EnterState(this);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(AIEyes.position,AIEyes.position + AIEyes.forward * lookRange);
    }
}
