using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LCControl : MonoBehaviour
{
    // Components
    [Tooltip("The Animator used by this GameObject")]
    [SerializeField]
    private Animator animator;
    [Tooltip("The Rigidbody2D used by this GameObject")]
    [SerializeField]
    private Rigidbody2D body;
    [Tooltip("The BoxCollider2D used by this GameObject")]
    [SerializeField]
    private BoxCollider2D boxCollider;
    [Tooltip("The SpriteRenderer used by this GameObject")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Other serialized fields
    [Tooltip("The horizontal movement increment of this GameObject")]
    [SerializeField]
    private float horizMoveSpeed;
    [Tooltip("The maximum horizontal speed of this GameObject")]
    [SerializeField]
    private float maxHorizontalSpeed;
    [Tooltip("The maximum vertical speed of this GameObject")]
    [SerializeField]
    private float maxVerticalSpeed;
    [Tooltip("The artificial friction to be applied to this GameObject")]
    [SerializeField]
    private float friction;
    [Tooltip("The force to be applied when this GameObject jumps")]
    [SerializeField]
    private float jumpForce;
    [Tooltip("The horizontal force to be applied while wall jumping")]
    [SerializeField]
    private float wallJumpForce;
    [Tooltip("The object layer this GameObject should treat as the ground")]
    [SerializeField]
    private LayerMask groundLayerMask;
    [Tooltip("The object layer this GameObject should treat as something that" +
        "can be wall jumped off of")]
    [SerializeField]
    private LayerMask wallLayerMask;

    // Private fields
    private Vector2 moveInput;
    private float moveForce;
    private bool isGrounded;
    private bool canWallJump;
    private bool touchingWall;
    private bool touchingWallL;
    private bool touchingWallR;

    private void Awake()
    {
        // Make sure components are set
        if (animator == null || body == null || boxCollider == null ||
            spriteRenderer == null)
        {
            Debug.LogError("Missing at least one required component; " +
                "ensure an Animator, Rigidbody2D, BoxCollider2D, " +
                "and SpriteRenderer are all assigned to this script");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        // Check if on ground
        CheckGrounded();

        // Check movement direction
        CheckDirection();

        // Check if falling
        CheckFalling();

        // Check if touching a wall
        CheckWallJump();
    }

    void FixedUpdate()
    {
        // Call move function
        Move(moveInput);

        // Check if vertical speed maximum has been reached
        CapVerticalSpeed();

        // Update animation if moving horizontally
        CheckRunning();
    }

    private void OnValidate()
    {
        moveForce = body.mass * horizMoveSpeed;
    }

    public void MoveActionPerformed(InputAction.CallbackContext context)
    {
        // Extract x value
        moveInput = context.ReadValue<Vector2>() * Vector2.right;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Check if jump key was pressed
        if (context.performed)
        {
            // Make sure player is able to jump
            if (isGrounded)
            {
                // Add force to jump
                body.AddForce(body.mass * jumpForce * Vector2.up,
                    ForceMode2D.Impulse);

                // Jump animation
                animator.SetTrigger("Jump");
            }
            else if (canWallJump && touchingWall)
            {
                //Upwards force
                body.AddForce(body.mass * jumpForce * Vector2.up,
                    ForceMode2D.Impulse);

                // Add bounce-off
                float force = (Mathf.Abs(body.velocity.x) + wallJumpForce) *
                    body.mass;
                float dir;
                if (touchingWallR)
                {
                    dir = -1;
                }
                else
                {
                    dir = 1;
                }
                body.AddForce(dir * force * Vector2.right, ForceMode2D.Impulse);

                // Wall jump animation
                animator.SetTrigger("Dash");

                // Prevent further wall jumps until grounded again
                canWallJump = false;
            }
        }
        else if (context.canceled)
        {
            // Cancel the jump if the player is jumping
            if (body.velocity.y > 0)
            {
                body.AddForce(.5f * body.mass * body.velocity.y * Vector2.down,
                    ForceMode2D.Impulse);
            }
        }
    }

    private void Move(Vector2 direction)
    {
        // Only move if there is a non-zero value for direction
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            // Calculate difference from maximum horizontal speed
            float speedDiff = maxHorizontalSpeed - Mathf.Abs(body.velocity.x);

            // Don't do anything if too close to maximum horizontal speed
            if (!Mathf.Approximately(speedDiff, 0f))
            {
                // Accelerate if speed difference is positive
                if (speedDiff > 0f)
                {
                    float accel = Mathf.Min(speedDiff / Time.fixedDeltaTime *
                        body.mass, moveForce);
                    body.AddForce(direction * Vector2.right * accel,
                        ForceMode2D.Force);
                }
                else // Decelerate if speed difference is negative
                {
                    body.AddForce(speedDiff * Mathf.Sign(body.velocity.x) *
                        body.mass * Vector2.right, ForceMode2D.Impulse);
                }
            }
        }
        else if (isGrounded) // Apply drag if no movement is being added
        {
            float amount = Mathf.Min(Mathf.Abs(body.velocity.x),
                Mathf.Abs(friction));
            amount *= Mathf.Sign(body.velocity.x);
            body.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    private void CheckRunning()
    {
        animator.SetFloat("Horizontal Speed", Mathf.Abs(body.velocity.x));
    }

    private void CheckGrounded()
    {
        // Use a box cast to check for ground
        RaycastHit2D boxCastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.down, .1f, groundLayerMask);

        // Returns null if the box cast failed (i.e. no ground)
        isGrounded = (boxCastHit.collider != null);

        // Set the "Is Grounded" parameter in the GameObject's Animator
        animator.SetBool("Is Grounded", isGrounded);

        // reset wall jump
        if (isGrounded)
        {
            canWallJump = true;
        }
    }

    private void CheckDirection()
    {
        // Don't check direction if no notable movement is taking place
        if (!Mathf.Approximately(body.velocity.x, 0f))
        {
            if (body.velocity.x > 0.01f) // Face right if moving right
            {
                spriteRenderer.flipX = false;
            }
            else if (body.velocity.x < 0.01f) // Face left if moving left
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void CheckFalling()
    {
        bool isFalling = false;
        // Check if player is falling (and not on a moving platform going
        // downward); if Y velocity is 0 while not grounded we will be falling
        // on the next frame
        if (!isGrounded && body.velocity.y <= 0)
        {
            isFalling = true;
        }
        animator.SetBool("Falling", isFalling);
    }

    private void CapVerticalSpeed()
    {
        // Calculate difference between max vertical speed and current speed
        float speedDiff = maxVerticalSpeed - Mathf.Abs(body.velocity.y);

        // If current speed is excessive, decelerate until it matches
        if (speedDiff < 0f)
        {
            body.AddForce(speedDiff * -Mathf.Sign(body.velocity.y) * body.mass *
                Vector2.down, ForceMode2D.Impulse);
        }
    }

    private void CheckWallJump()
    {
        // Use two box casts to check for walls
        RaycastHit2D boxCastHitL = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.left, .1f, wallLayerMask);
        RaycastHit2D boxCastHitR = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.right, .1f, wallLayerMask);

        // Check if either box cast succeeded
        touchingWallL = (boxCastHitL.collider != null);
        touchingWallR = (boxCastHitR.collider != null);

        // Returns null if both box casts failed (i.e. no wall)
        touchingWall = (touchingWallL || touchingWallR);
    }
}
