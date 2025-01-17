using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EllenPlayerControl : MonoBehaviour
{
    [Tooltip("The animator used by this game object")]
    [SerializeField]
    private Animator animator;
    [Tooltip("The rigidbody used by this game object")]
    [SerializeField]
    private Rigidbody2D body;

    // Field for user input
    private Vector2 moveInput;

    [Tooltip("Maximum force applied to the rigidbody")]
    [SerializeField]
    private float moveSpeed;

    [Tooltip("Amount of acceleration to apply to get to move speed")]
    [SerializeField]
    private float moveAcceleration;

    private float moveForce;

    [Tooltip("Maximum vertical speed (jumping or falling")]
    [SerializeField]
    private float maxVerticalSpeed;

    // Public property for maxForce
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    [Tooltip("The box collider used by this game object")]
    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private LayerMask envLayerMask;
    [SerializeField]
    private LayerMask pushLayerMask;

    private bool isGrounded; // Whether the player is grounded
    private bool isPushing; // Whether the player is pushing something
    private bool canWallJump; // Whether the player is able to wall jump

    [Tooltip("Force to apply to this game object when jumping")]
    [SerializeField]
    private float jumpForce;

    [Tooltip("Artificial friction to apply to this game object when not moving")]
    [SerializeField]
    private float friction;

    [Tooltip("The Sprite Renderer used by this game object")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Awake is called when the object is first constructed
    private void Awake()
    {
        // Make sure required components are present
        if (animator == null)
        {
            Debug.LogError("Could not find Animator");
        }
        if (body == null)
        {
            Debug.LogError("Could not find Rigidbody2D");
        }
        if (boxCollider == null)
        {
            Debug.LogError("Could not find BoxCollider2D");
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("Could not find SpriteRenderer - attached object may be incompatible");
        }
        moveForce = body.mass * moveAcceleration;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is grounded
        CheckGrounded();

        // Check if wall jumping is possible
        CheckWall();

        // Check movement direction
        CheckDirection();

        // Check if falling
        CheckFalling();

        // Check if pushing
        CheckPushing();
    }

    private void OnValidate()
    {
        moveForce = body.mass * moveAcceleration;
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
            if (isGrounded || canWallJump)
            {
                // Add force to jump
                body.AddForce(Vector2.up * jumpForce * body.mass, ForceMode2D.Impulse);

                // Set trigger for jumping
                animator.SetTrigger("Jump");
            }
        }
        else if (context.canceled)
        {
            // Cancel the jump if player is jumping
            if (body.velocity.y > 0)
            {
                body.AddForce(.5f * body.mass * body.velocity.y * Vector2.down,
                    ForceMode2D.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        // Call move fuction
        Move(moveInput);

        // Check if vertical speed maximum has been reached
        CapVerticalSpeed();

        // Update animation if horizontal movement is present
        CheckRunning();
    }

    private void Move(Vector2 direction)
    {
        // Only move if there is a non-zero value for direction
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            // Calculate maximum speed difference
            float speedDiff = moveSpeed - Mathf.Abs(body.velocity.x);
            if (!Mathf.Approximately(speedDiff, 0f))
            {
                // Accelerate
                if (speedDiff > 0f)
                {
                    float accelCap =
                        Mathf.Min(speedDiff / Time.fixedDeltaTime * body.mass,
                        moveForce);
                    body.AddForce(direction * Vector2.right * accelCap,
                        ForceMode2D.Force);
                }
                else
                {
                    // Decelerate
                    body.AddForce(speedDiff *
                        Mathf.Sign(body.velocity.x) * body.mass * Vector2.right,
                        ForceMode2D.Impulse);
                }
            }
        }
        else if (isGrounded)
        {
            float amount = Mathf.Min(Mathf.Abs(body.velocity.x),
                Mathf.Abs(friction));
            amount *= Mathf.Sign(body.velocity.x);
            body.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    // Updates the Animator's field for hoizontal movement
    private void CheckRunning()
    {
        animator.SetFloat("MovingSpeed", Mathf.Abs(body.velocity.x));
    }

    private void CheckGrounded()
    {
        RaycastHit2D boxCastHit = Physics2D.BoxCast(
            boxCollider.bounds.center, boxCollider.bounds.size, 0f,
            Vector2.down, .1f, envLayerMask);

        // Returns null if the box cast failed
        isGrounded = (boxCastHit.collider != null);

        // Set the IsGrounded parameter in the Animator
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void CheckDirection()
    {
        // If nonneglible movement is happening in either direction,
        // the sprite should face that direction
        if (!Mathf.Approximately(body.velocity.x, 0f))
        {
            if (body.velocity.x > 0.01f)
            {
                spriteRenderer.flipX = false; // Face right
            }
            else if (body.velocity.x < 0.01f)
            {
                spriteRenderer.flipX = true; // Face left
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
        animator.SetBool("IsFalling", isFalling);
    }

    private void CheckPushing()
    {
        RaycastHit2D boxCastHitL = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.left, .1f, pushLayerMask);
        RaycastHit2D boxCastHitR = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.right, .1f, pushLayerMask);

        // Returns null if the box cast failed
        isPushing = (boxCastHitL.collider != null ||
            boxCastHitR.collider != null);

        // Set the IsPushing parameter in the Animator
        animator.SetBool("IsPushing", isPushing);
    }

    private void CheckWall()
    {
        RaycastHit2D boxCastHitL = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.left, .1f, envLayerMask);
        RaycastHit2D boxCastHitR = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0f, Vector2.right, .1f, envLayerMask);

        // Returns null if the box cast failed
        canWallJump = (boxCastHitL.collider != null ||
            boxCastHitR.collider != null);
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
}
