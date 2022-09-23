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

    // Public property for maxForce
    public float MoveSpeed { get { return moveSpeed; }
        set { moveSpeed = value; } }

    [Tooltip("The box collider used by this game object")]
    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private LayerMask envLayerMask;

    private bool isGrounded; // Whether the player is grounded

    [Tooltip("Force to apply to this game object when jumping")]
    [SerializeField]
    private float jumpForce;

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

        moveForce = body.mass * moveAcceleration;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is grounded; update animation accordingly
        CheckGrounded();
    }

    private void OnValidate()
    {
        moveForce = body.mass * moveAcceleration;
    }

    public void MoveActionPerformed(InputAction.CallbackContext context)
    {
        // Extract x value
        moveInput = new Vector2(context.ReadValue<Vector2>().x, 0);
        Debug.Log("Got move input of " + context.ReadValue<Vector2>().x);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Check if jump key was pressed
        if (context.performed)
        {
            if (isGrounded)
            {
                // Add force to jump
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

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

        // Update animation if horizontal movement is present
        CheckRunning();
    }

    private void Move(Vector2 direction)
    {
        // Only move if there is a non-zero value for direction
        if (!Mathf.Approximately(direction.x, 0))
        {
            // Calculate maximum speed difference
            float speedDiff = moveSpeed - Mathf.Abs(body.velocity.x);
            Debug.Log("Difference from maximum speed: " + speedDiff);
            if (!Mathf.Approximately(speedDiff, 0))
            {
                if (speedDiff > 0)
                {
                    float accelCap =
                        Mathf.Min(speedDiff / Time.fixedDeltaTime * body.mass,
                        moveAcceleration);
                    Debug.Log("Adding force of " + moveAcceleration);
                    body.AddForce(direction * accelCap, ForceMode2D.Force);
                }
            }
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

        // This returns null if the box cast failed
        isGrounded = (boxCastHit.collider != null);

        // Set the IsGrounded parameter in the Animator
        animator.SetBool("IsGrounded", isGrounded);
    }
}
