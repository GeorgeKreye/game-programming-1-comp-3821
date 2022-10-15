using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI that patrols an area, turning around upon reaching an obstacle or edge
/// </summary>
public class ObstaclePatrol : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("The Rigidbody2D of this GameObject")]
    [SerializeField] private Rigidbody2D body;
    [Tooltip("The BoxCollider2D of this GameObject")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Fields")]
    [Tooltip("The movmement speed increment of the patrol")]
    [SerializeField] private float moveSpeed;
    [Tooltip("The maximum speed this GameObject can move at")]
    [SerializeField] private float maxSpeed;
    [Tooltip("Whether movement should start reversed (facing left)")]
    [SerializeField] private bool startReversed;
    [Tooltip("The LayerMask to treat as ground")]
    [SerializeField] private LayerMask groundLayerMask;
    [Tooltip("The LayerMask to treat as an obstacle")]
    [SerializeField] private LayerMask obstacleLayerMask;

    /// <summary>
    /// Whether movement is reversed (moving left)
    /// </summary>
    private bool reversed;

    // Start is called before the first frame update
    void Start()
    {
        // Initial movement direction (reversed = left, !reversed = right)
        reversed = startReversed;
    }

    // FixedUpdate is called once per fixed framerate frame
    void FixedUpdate()
    {
        // Apply movement if not paused
        if (GameManager.Instance.CurrentPauseState ==
            GameManager.PauseState.Playing)
        {
            // Move
            Move();

            // Enforce max speed
            CapHorizontalSpeed();
        }

        // Check for wall
        CheckForObstacle();

        // Check for edge
        CheckForEdge();
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

    /// <summary>
    /// Checks if there is ground near the GameObject
    /// </summary>
    void CheckForEdge()
    {
        //Make raycasts
        RaycastHit2D leftEdge = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, new Vector2(-1, -1), .1f,
            groundLayerMask);
        RaycastHit2D rightEdge = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, new Vector2(1, -1), .1f,
            groundLayerMask);

        // Change direction if either raycast failed
        if (leftEdge.collider == null || rightEdge.collider == null)
        {
            Debug.Log("Drop found");
            // Change direction
            if (leftEdge.collider == null)
            {
                // Go right
                reversed = false;
            }
            else
            {
                // Go left
                reversed = true;
            }

            // Stop current movement
            body.AddForce(body.velocity.x * body.mass * Vector2.left,
                ForceMode2D.Impulse);
        }
    }

    // Collsion handling
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collsion is with ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Do nothing
            return;
        }

        // Assume collision is in direction of movement

        // Change direction
        reversed = !reversed;

        // Stop current movement
        body.AddForce(body.velocity.x * body.mass * Vector2.left,
                ForceMode2D.Impulse);
    }

    /// <summary>
    /// Checks for obstacle collisions
    /// </summary>
    void CheckForObstacle()
    {
        // Check for obstacles
        RaycastHit2D castHitL = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left,
            boxCollider.bounds.extents.x,obstacleLayerMask);
        RaycastHit2D castHitR = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right,
            boxCollider.bounds.extents.x,obstacleLayerMask);

        // Change direction if a wall is detected
        if (castHitL.collider != null || castHitR.collider != null)
        {
            Debug.Log("Obstacle found");
            // Change direction
            if (castHitL.collider != null)
            {
                // Go right
                reversed = false;
            }
            else
            {
                // Go left
                reversed = true;
            }

            // Stop current movement
            body.AddForce(body.velocity.x * body.mass * Vector2.left,
                ForceMode2D.Impulse);
        }
    }
}
