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
    [Tooltip("Maximum visual distance off of sightline")]
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
    [Tooltip("The attack range used when attempting to damage a target")]
    public float attackRange = 2f;
    [Tooltip("Maximum distance from attack cast to count as still hitting")]
    public float attackRadius = 5f;
    [Tooltip("The Animator used by this GameObject")]
    public Animator animator;
    [Tooltip("The position occupied by this GameObject last frame")]
    public Vector3 prevPosition;
    [Tooltip("Whether the animator should render the character running")]
    public bool isRunning = false;
    [Tooltip("The walking speed of the agent")]
    public float walkSpeed = 3.5f;
    [Tooltip("The run speed of the agent")]
    public float runSpeed = 7f;
    [Tooltip("The collider used by this GameObject")]
    public Collider objectCollider;
    [Tooltip("The Rigidbody used by this GameObject")]
    public Rigidbody objectRigidbody;
    [Tooltip("The active GameManager instance")]
    public GameManager gameManager;
    [Tooltip("The number of patrol cycles that have occured")]
    public int patrolCycles = 0;
    [Tooltip("Whehter the AI has attacked once or more")]
    public bool hasAttacked = false;
    [Tooltip("The threshold at which to consider the AI approximately at " +
        "a point")]
    public float threshold = 0f;
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
        // Get game manager instance
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Call update actions
        if (isActive)
        {
            // Update current state
            currentState.UpdateState(this);
        }

        // Update animator movement animation
        if (animator != null)
        {
            UpdateMovementAnimation();
        }

        // Update previous position for anything referencing it
        prevPosition = transform.position;

        // Update timer
        timer += Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        if (AIEyes != null)
        {
            // Draw sightlines
            Gizmos.color = Color.red;
            Gizmos.DrawLine(AIEyes.position, AIEyes.position + AIEyes.forward *
                lookRange);
            Gizmos.DrawSphere(AIEyes.position + AIEyes.forward * lookRange,
                lookRadius);
        }
    }
    #endregion

    /// <summary>
    /// Sets up the FSM
    /// </summary>
    public void Setup()
    {
        /* Doesn't work
        // Don't adjust NavMesh if no agent is attached
        if (agent != null)
        {
            // Hit container
            NavMeshHit hit;

            // Make sure home waypoint is on the navigation mesh
            if (homeWaypoint != null)
            {
                NavMesh.SamplePosition(homeWaypoint.position, out hit,
                    checkRadius, 0);
                homeWaypoint.position = hit.position;
            }

            // Make sure patrol waypoints are on the navigation mesh
            if (patrolWaypoints.Length > 0)
            {
                foreach (Transform waypoint in patrolWaypoints)
                {
                    NavMesh.SamplePosition(waypoint.position, out hit,
                        checkRadius,0);
                    waypoint.position = hit.position;
                }
            }
        }
           Doesn't work */
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

            // Reset patrol cycles
            patrolCycles = 0;

            // Restart wander
            wanderRestart = true;

            // Perform enter actions of new state
            currentState.EnterState(this);
        }
    }

    /// <summary>
    /// Updates horizontal speed and whether the character is running
    /// for the Animator
    /// </summary>
    private void UpdateMovementAnimation()
    {
        // Calculate and pass horizontal speed to animator
        animator.SetFloat("HorizontalSpeed", Vector3.Magnitude(
            transform.position - prevPosition));

        // Send whether to play running animation instead of walking to
        // animator
        animator.SetBool("Running", isRunning);
    }
}
