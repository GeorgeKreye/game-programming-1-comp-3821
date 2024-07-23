using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for moving platform behavior
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Whether this moving platform is active")]
    public bool active = false;

    #region Parameters
    [Header("Parameters")]
    [Tooltip("Endpoints of platform movement")]
    [SerializeField] private List<Vector3> waypoints;
    [Tooltip("The movement speed of the platform")]
    [SerializeField] private float moveSpeed = 3f;
    [Tooltip("The movement acceleartion of the platform")]
    [SerializeField] private float moveAcceleration = 3f;
    #endregion

    #region Components
    [Header("Components")]
    [Tooltip("The Rigidbody used by this GameObject")]
    [SerializeField] private Rigidbody objectRigidbody;
    [Tooltip("The colldier used by this GameObject")]
    [SerializeField] private Collider objectCollider;
    [Tooltip("The timer to use for pausing at endpoints")]
    [SerializeField] private Timer pauseTimer;
    #endregion

    /// <summary>
    /// The endpoint the platform is currently moving towards
    /// </summary>
    private int currentWaypoint = 0;

    /// <summary>
    /// Whether movement is currently paused
    /// </summary>
    private bool paused = false;

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        // Add start position to waypoints
        waypoints.Add(transform.position);

        // Make sure rigidbody is kinematic
        if (!objectRigidbody.isKinematic)
        {
            // Change rigidbody to kinematic
            Debug.LogError("Rigidbody is not kinematic; auto-correcting");
            objectRigidbody.isKinematic = true;
        }

        // Make sure collider is present
        if (objectCollider == null)
        {
            Debug.LogError("Could not find a collider");
        }
    }

    // FixedUpdate is called once per fixed frame
    void FixedUpdate()
    {
        // Perform movement behavior
        Movement();

        // Cap velocity to movement speed
        CapSpeed();
    }

    #region Player Collision
    void OnCollisionStay(Collision collision)
    {
        // Check if colliding with player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Impart movement to player
            Vector3 moveDir = objectRigidbody.velocity;
            collision.rigidbody.AddForce(moveDir * collision.rigidbody.mass,
                ForceMode.Impulse);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if colliding with player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cancel imparted movement on player
            Vector3 moveDir = objectRigidbody.velocity;
            collision.rigidbody.AddForce(-moveDir * collision.rigidbody.mass,
                ForceMode.Impulse);
        }
    }
    #endregion
    #endregion

    #region Event Methods
    /// <summary>
    /// Called when the timer has expired; should listen for timer event
    /// of the same name
    /// </summary>
    public void OnTimerExpired()
    {
        // End pause
        paused = false;

        // Reset timer
        pauseTimer.ResetTimer();
    }

    #region Activation/Deactivation
    /// <summary>
    /// Called when moving platform is to be activated; should listen for an
    /// event
    /// </summary>
    public void OnActivationTrigger()
    {
        // Set as active
        active = true;
    }

    /// <summary>
    /// Called when moving platform is to be deactivated; should listen for an
    /// event
    /// </summary>
    public void OnDeactivationTrigger()
    {
        // Set as inactive
        active = false;
    }
    #endregion
    #endregion

    /// <summary>
    /// Performs movement behavior of the moving platform
    /// </summary>
    private void Movement()
    {
        // Perform movement behavior if active or not at endpoint
        if (active)
        {
            // Check if at endpoint
            if (Mathf.Approximately(Vector3.Distance(objectRigidbody.position,
            waypoints[currentWaypoint]), 0f))
            {
                // Cancel movement
                objectRigidbody.AddForce(-objectRigidbody.velocity,
                    ForceMode.VelocityChange);

                // Increment current waypoint
                currentWaypoint = (currentWaypoint + 1) % waypoints.Count;

                // Pause if pauseTimer is set
                if (pauseTimer != null)
                {
                    paused = true;
                    pauseTimer.StartTimer();
                }

                if (!paused)
                {
                    // Calculate movement direction and speed
                    Vector3 moveDir = Vector3.MoveTowards(
                        objectRigidbody.position, waypoints[currentWaypoint],
                        1).normalized;
                    float moveForce = Mathf.Min(moveAcceleration,
                        Vector3.Distance(objectRigidbody.position,
                        waypoints[currentWaypoint])) * objectRigidbody.mass;

                    // Apply movement
                    objectRigidbody.AddForce(moveDir * moveForce,
                        ForceMode.Impulse);
                }

            }
        }
        else if (!Mathf.Approximately(Vector3.Distance(objectRigidbody.position,
            waypoints[currentWaypoint]), 0f))
        {
            // Calculate movement direction and speed
            Vector3 moveDir = Vector3.MoveTowards(objectRigidbody.position,
                waypoints[currentWaypoint], 1).normalized;
            float moveForce = Mathf.Min(moveAcceleration,
                Vector3.Distance(objectRigidbody.position,
                waypoints[currentWaypoint])) * objectRigidbody.mass;

            // Apply movement
            objectRigidbody.AddForce(moveDir * moveForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Enforces move speed maximum
    /// </summary>
    private void CapSpeed()
    {
        // Get difference from max move speed
        float speedDiff = moveSpeed -
            Mathf.Abs(objectRigidbody.velocity.magnitude);

        // Slow speed if maxSpeed significantly exceeded
        if (!Mathf.Approximately(speedDiff, 0f) && speedDiff < 0)
        {
            objectRigidbody.AddForce(objectRigidbody.mass * speedDiff *
                objectRigidbody.velocity.normalized, ForceMode.Impulse);
        }
    }
}
