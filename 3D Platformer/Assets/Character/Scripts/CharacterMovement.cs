using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseMovement
{
    [Header("Components/Objects")]

    [Tooltip("The Rigidbody of the character")]
    [SerializeField] private Rigidbody characterRigidbody;

    [Tooltip("The capsule collider of the character")]
    [SerializeField] private CapsuleCollider characterCollider;

    [Tooltip("The Animator used on the character model associated with the " +
        "character")]
    [SerializeField] private Animator animator;

    [Tooltip("The transform of the character model associated with the " +
        "character")]
    [SerializeField] private Transform characterModel;

    [Header("Ground Movement")]
    [Tooltip("The speed that this character accelerates at")]
    [SerializeField] float moveAcceleration = 60f;

    [Tooltip("The current max speed of this character")]
    [SerializeField] private float moveSpeed = 7f;

    [Tooltip("The speed to interpolate rotation at")]
    [SerializeField] private float rotationSpeed = 5f;

    [Tooltip("The layer mask to treat as ground")]
    [SerializeField] private LayerMask groundLayerMask;

    [Tooltip("The distance from the bottom of the character to check for " +
        "ground at")]
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Tooltip("The distance from the front of the character to check for " +
        "slopes at")]
    [SerializeField] private float slopeCheckDistance;

    [Tooltip("The maximum slope height that the player can climb")]
    [SerializeField] private float maxSlopeHeight = 0.25f;

    [Header("Jumping")]
    [Tooltip("The force to apply when jumping")]
    [SerializeField] private float jumpForce = 5f;

    [Tooltip("Multiplier for airbone horizontal movement")]
    [SerializeField] private float airControlMultiplier = 1f;

    [Tooltip("The maximum vertical speed")]
    [SerializeField] private float maxVerticalMoveSpeed = 25f;

    private Vector3 cameraAdjustedInputDirection;

    /// <summary>
    /// Whether the character is on the ground
    /// </summary>
    private bool isGrounded;

    /// <summary>
    /// Whether the character was on the ground last frame
    /// </summary>
    private bool wasGroundedLastFrame;

    /// <summary>
    /// Whether the character is falling down
    /// </summary>
    private bool isFalling;

    // called by fixed update
    private void MoveCharacter()
    {
        // move character w/ controller
        if (cameraAdjustedInputDirection != Vector3.zero)
        {
            // apply force to character to create movement
            if (isGrounded)
            {
                // check for slope
                if (ClimbableSlope())
                {
                    Vector3 slopeAdjustedInputDirection = new(
                        cameraAdjustedInputDirection.x,
                        cameraAdjustedInputDirection.y + maxSlopeHeight,
                        cameraAdjustedInputDirection.z);
                    characterRigidbody.AddForce(characterRigidbody.mass *
                        moveAcceleration * slopeAdjustedInputDirection,
                        ForceMode.Force);
                }
                else
                {
                    characterRigidbody.AddForce(characterRigidbody.mass *
                        moveAcceleration * cameraAdjustedInputDirection,
                        ForceMode.Force);
                }
            }
            else
            {
                characterRigidbody.AddForce(airControlMultiplier *
                    characterRigidbody.mass * moveAcceleration *
                    cameraAdjustedInputDirection, ForceMode.Force);
            }
        }
    }

    private Vector3 GetHorizontalRBVelocity()
    {
        return new Vector3(characterRigidbody.velocity.x, 0,
            characterRigidbody.velocity.z);
    }

    private float getMaxVelocity()
    {
        return moveSpeed * cameraAdjustedInputDirection.magnitude;
    }

    private void LimitVelocity()
    {
        Vector3 currentVelocity = GetHorizontalRBVelocity();

        float maxAllowedVelocity = getMaxVelocity();

        if (currentVelocity.sqrMagnitude > (maxAllowedVelocity *
            maxAllowedVelocity))
        {
            Vector3 counteractDirection = currentVelocity.normalized * -1f;
            float counteractAmount = currentVelocity.magnitude -
                maxAllowedVelocity;
            characterRigidbody.AddForce(characterRigidbody.mass *
                counteractAmount * counteractDirection, ForceMode.Impulse);
        }

        if (!isGrounded)
        {
            if (Mathf.Abs(characterRigidbody.velocity.y) > maxVerticalMoveSpeed)
            {
                Vector3 counteractDirection = -1f *
                    Mathf.Sign(characterRigidbody.velocity.y) * Vector3.up;
                float counteractAmount = Mathf.Abs(
                    characterRigidbody.velocity.y) - maxVerticalMoveSpeed;
                characterRigidbody.AddForce(characterRigidbody.mass *
                    counteractAmount * counteractDirection, ForceMode.Impulse);
            }
        }
    }

    private void CheckGrounded()
    {
        // cache last frame grounded state
        wasGroundedLastFrame = isGrounded;

        // check if grounded
        Vector3 overlapSphereOrigin = transform.position + (Vector3.up *
            (characterCollider.radius - groundCheckDistance));
        Collider[] overlappedColliders = Physics.OverlapSphere(
            overlapSphereOrigin, characterCollider.radius * 0.95f,
            groundLayerMask, QueryTriggerInteraction.Ignore);
        isGrounded = (overlappedColliders.Length > 0);

        // update animation
        animator.SetBool("Grounded", isGrounded);
    }

    private bool ClimbableSlope()
    {
        bool result = false;

        // check if against an object
        bool collisionCastHitF = Physics.Raycast(transform.position,
            Vector3.forward, characterCollider.radius + slopeCheckDistance);
        bool collisionCastHitL = Physics.Raycast(transform.position,
            Vector3.left, characterCollider.radius + slopeCheckDistance);
        bool collisionCastHitR = Physics.Raycast(transform.position,
            Vector3.right, characterCollider.radius + slopeCheckDistance);
        bool collisionCastHitB = Physics.Raycast(transform.position,
            Vector3.back, characterCollider.radius + slopeCheckDistance);

        // return if not against an object
        if (!collisionCastHitF && !collisionCastHitL && !collisionCastHitR &&
            !collisionCastHitB)
        {
            return result;
        }

        // check if climbable slope
        Vector3 maxHeightCastOrigin = new Vector3(transform.position.x,
            transform.position.y + maxSlopeHeight + 0.01f,
            transform.position.z);
        bool maxHeightCastHit = false;
        if (collisionCastHitF) // forward
        {
            maxHeightCastHit = Physics.Raycast(maxHeightCastOrigin,
                Vector3.forward, characterCollider.radius + slopeCheckDistance);
        }
        else if (collisionCastHitL) // left
        {
            maxHeightCastHit = Physics.Raycast(maxHeightCastOrigin,
                Vector3.left, characterCollider.radius + slopeCheckDistance);
        }
        else if (collisionCastHitR) // right
        {
            maxHeightCastHit = Physics.Raycast(maxHeightCastOrigin,
                Vector3.right, characterCollider.radius + slopeCheckDistance);
        }
        else if (collisionCastHitB) // backward
        {
            maxHeightCastHit = Physics.Raycast(maxHeightCastOrigin,
                Vector3.back, characterCollider.radius + slopeCheckDistance);
        }
        result = !maxHeightCastHit;

        // return
        return result;
    }

    private void GroundAnimation()
    {
        // get magniude and send to animator component
        float magnitude = GetHorizontalRBVelocity().magnitude;
        animator.SetFloat("Horizontal Speed", magnitude);
    }

    private void CheckFalling()
    {
        isFalling = (!isGrounded && characterRigidbody.velocity.y < 0.1);
        animator.SetBool("Falling", isFalling);
    }

    #region BaseMovement Functions
    override public void Move(Vector3 moveDir)
    {
        cameraAdjustedInputDirection = moveDir;
    }

    override public void RotateCharacter()
    {
        if (cameraAdjustedInputDirection != Vector3.zero)
        {
            // face in movement dir
            characterModel.forward =
                Vector3.Slerp(characterModel.forward,
                cameraAdjustedInputDirection.normalized,
                Time.deltaTime * rotationSpeed);
        }
    }

    override public void Jump()
    {
        // make sure jump is being performed while grounded
        if (isGrounded)
        {
            // calculate adjusted jump force
            float jumpImpulse = jumpForce * characterRigidbody.mass;

            // perform jump
            characterRigidbody.AddForce(Vector3.up * jumpImpulse,
                ForceMode.Impulse);

            // send jump to animator component
            animator.SetTrigger("Jump");
        }
    }

    override public void JumpCanceled()
    {

    }
    #endregion
    #region Unity Functions
    // Start is called before the first frame update
    override protected void Start()
    {

    }

    // Update is called once per frame
    override protected void Update()
    {

    }

    override protected void FixedUpdate()
    {

        // check if grounded
        CheckGrounded();

        // check if falling
        CheckFalling();

        // move
        MoveCharacter();

        // cap velocity
        LimitVelocity();

        // update ground animation
        GroundAnimation();

        // update rotation
        RotateCharacter();

    }
    #endregion
}

