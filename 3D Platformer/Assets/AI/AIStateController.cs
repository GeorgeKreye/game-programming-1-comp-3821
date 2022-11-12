using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for an AI Agent to handle AI states
/// </summary>
public class AIStateController : MonoBehaviour
{
    #region AI State
    [Header("State Control")]
    [Tooltip("The current state the FSM is in")]
    public AIState currentState;
    [Tooltip("Whehter this FSM is active")]
    public bool isActive;
    #endregion

    #region Blackboard Fields
    [Header("Blackboard")]
    [Tooltip("The NavMeshAgent used by this controller")]
    public NavMeshAgent agent;
    [Tooltip("The position to use as the 'home' waypoint")]
    public Transform homeWaypoint;
    [Tooltip("Position to use as eye level")]
    public Transform AIEyes;
    [Tooltip("Maximum visual distance off of a sightline")]
    public float lookRadius = 5f;
    [Tooltip("Maximum visual range")]
    public float lookRange = 10f;
    [Tooltip("Target object for tracking (usualy the player)")]
    public Transform chaseTarget;
    [Tooltip("Maximum radius when wandering")]
    public float wanderRadius = 30f;
    [Tooltip("Whether to calculate a new wander target")]
    public bool wanderRestart = true;
    [Tooltip("Timer for waiting")]
    public float timer = 0f;
    [Tooltip("Positions to use for patrolling")]
    public Transform[] patrolWaypoints;
    [Tooltip("The current patrol waypoint")]
    public int currentPWaypoint = 0;
    [Tooltip("The maximum radius to check for NavMesh hits in")]
    public float checkRadius = 10f;
    [Tooltip("The duration of the pause between patrol segments")]
    public float patrolPauseDuration = 30f;
    #endregion


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
        // Don't adjust NavMesh if no agent is attached
        if (agent != null)
        {
            // Hit container
            NavMeshHit hit;

            // Make sure home waypoint is on the navigation mesh
            if (homeWaypoint != null)
            {
                NavMesh.SamplePosition(homeWaypoint.position, out hit, 10f, 0);
                homeWaypoint.position = hit.position;
            }

            // Make sure patrol waypoints are on the navigation mesh
            if (patrolWaypoints.Length > 0)
            {
                foreach (Transform waypoint in patrolWaypoints)
                {
                    NavMesh.SamplePosition(waypoint.position, out hit, checkRadius, 0);
                    waypoint.position = hit.position;
                }
            }
        }
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

            // Restart wander
            wanderRestart = true;

            // Perform enter actions of new state
            currentState.EnterState(this);
        }
    }

    private void OnDrawGizmos()
    {
        if (AIEyes != null)
        {
            // Draw sightlines
            Gizmos.color = Color.red;
            Gizmos.DrawLine(AIEyes.position, AIEyes.position + AIEyes.forward * lookRange);
            Gizmos.DrawSphere(AIEyes.position + AIEyes.forward * lookRange, lookRadius);
        }
    }
}
