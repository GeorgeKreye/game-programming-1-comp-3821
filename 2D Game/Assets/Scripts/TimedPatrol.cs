using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPatrol : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("The Timer script to use for timing patrol")]
    [SerializeField] private Timer patrolTimer;
    [Tooltip("The Rigidbody2D attached to this GameObject")]
    [SerializeField] private Rigidbody2D body;

    [Header("Fields")]
    [Tooltip("The duration of each patrol")]
    [SerializeField] private float duration;
    [Tooltip("Whether movement should start reversed (facing left)")]
    [SerializeField] private bool startReversed;
    [Tooltip("The movement speed of the patrol")]
    [SerializeField] private float moveSpeed;
    [Tooltip("The maximum speed this GameObject can move at")]
    [SerializeField] private float maxSpeed;

    /// <summary>
    /// Whether movement direction is reversed
    /// </summary>
    private bool reversed;

    void Start()
    {
        // Set initial direction (reversed = left, !reversed = right)
        reversed = startReversed;

        // Create timer
        patrolTimer.Duration = duration;
        patrolTimer.StartTimer();
    }

    void FixedUpdate()
    {
        // Move if not paused
        if (GameManager.Instance.CurrentPauseState ==
            GameManager.PauseState.Playing)
        {
            // Move
            Move();

            // Enforce max speed
            CapHorizontalSpeed();
        }

    }

    /// <summary>
    /// Moves the GameObject
    /// </summary>
    private void Move()
    {
        // Determine direction
        Vector2 dir;
        if (reversed)
        {
            dir = Vector2.left;
        }
        else
        {
            dir = Vector2.right;
        }

        // Calculate force to add
        float force = moveSpeed * body.mass;

        // Add force
        body.AddForce(force * dir, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Reverses movement upon timer ending
    /// </summary>
    public void TimerExpired()
    {
        // Change direction
        if (reversed)
        {
            reversed = false;
        }
        else
        {
            reversed = true;
        }

        // Stop current movement
        body.AddForce(body.velocity.x * body.mass * Vector2.left,
            ForceMode2D.Impulse);

        // Restart timer
        patrolTimer.ResetTimer();
        patrolTimer.StartTimer();
    }

    /// <summary>
    /// Prevents the GameObject from moving faster than a set horizontal speed
    /// </summary>
    void CapHorizontalSpeed()
    {
        // Get speed difference
        float speedDiff = maxSpeed - Mathf.Abs(body.velocity.x);
        if (speedDiff < 0)
        {
            // Enforce max speed
            body.AddForce(Mathf.Abs(speedDiff) * -Mathf.Sign(body.velocity.x) *
                body.mass * Vector2.right, ForceMode2D.Impulse);
        }
    }
}
