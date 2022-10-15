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

    /// <summary>
    /// The SpriteRenderer used for this GameObject
    /// </summary>
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        // Initial movement direction (reversed = left, !reversed = right)
        reversed = startReversed;

        // Get spriteRenderer
        sprite = gameObject.GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("Could not find a SpriteRenderer on " + gameObject);
        }
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

    // Update is called once per frame
    void Update()
    {
        // update sprite direction
        sprite.flipX = reversed;
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
        // Calculate cast startpoints and distance
        Vector2 startL = new Vector2(transform.position.x -
            boxCollider.bounds.extents.x, transform.position.y);
        Vector2 startR = new Vector2(transform.position.x +
            boxCollider.bounds.extents.x, transform.position.y);
        float dist = 1f;

        //Make raycasts
        RaycastHit2D castHitL = Physics2D.Raycast(startL, Vector2.down, dist,
            groundLayerMask);
        RaycastHit2D castHitR = Physics2D.Raycast(startR, Vector2.down, dist,
            groundLayerMask);

        //DEBUG
        Vector2 endL = new Vector2(transform.position.x -
            boxCollider.bounds.extents.x, transform.position.y - dist);
        Vector2 endR = new Vector2(transform.position.x +
            boxCollider.bounds.extents.x, transform.position.y - dist);
        Debug.DrawLine(startL, endL, Color.magenta);
        Debug.DrawLine(startR, endR, Color.magenta);

        // Change direction if either raycast failed
        if (castHitL.collider == null || castHitR.collider == null)
        {
            Debug.Log("Drop found");
            // Change direction
            if (castHitL.collider == null)
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
            // (with a nudge to prevent being stuck on edge)
            if (!Mathf.Approximately(body.velocity.x,0f))
            {
                float nudge = 0.1f;
                if (!reversed)
                {
                    nudge = -nudge;
                }
                body.AddForce((body.velocity.x + nudge) * body.mass * Vector2.left,
                ForceMode2D.Impulse);
            }
            
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
        RaycastHit2D castHitL = Physics2D.Raycast(transform.position,
            Vector2.left, 1f, obstacleLayerMask);
        RaycastHit2D castHitR = Physics2D.Raycast(transform.position,
            Vector2.right, 1f, obstacleLayerMask);

        // DEBUG
        Vector2 endL = new Vector2(transform.position.x -
            (boxCollider.bounds.extents.x + 1f),transform.position.y);
        Vector2 endR = new Vector2(transform.position.x +
            (boxCollider.bounds.extents.x + 1f), transform.position.y);
        Debug.DrawLine(transform.position,endL,Color.green);
        Debug.DrawLine(transform.position,endR,Color.green);

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
