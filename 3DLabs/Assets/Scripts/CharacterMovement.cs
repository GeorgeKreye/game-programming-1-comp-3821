using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseMovement
{
    [Header("Ground Movement")]
    [Tooltip("The speed that this character accelerates at")]
    [SerializeField] float moveAcceleration = 60f;

    [Tooltip("The current max speed of this character")]
    [SerializeField] private float moveSpeed = 7f;

    [Tooltip("The friction decceleration of this character")]
    [SerializeField] private float friction = 60f;

    [SerializeField] private Vector3 cameraAdjustedInputDirection;

    [Tooltip("The Rigidbody of the character")]
    [SerializeField] private Rigidbody characterRigidbody;

    // called by fixed update
    private void MoveCharacter()
    {
        // move character w/ controller
        if (cameraAdjustedInputDirection != Vector3.zero)
        {
            // apply force to character to create movement
            characterRigidbody.AddForce(cameraAdjustedInputDirection *
                moveAcceleration * characterRigidbody.mass, ForceMode.Force);
        }
        else
        {
            // slow down
            if (!Mathf.Approximately(characterRigidbody.velocity.sqrMagnitude,
                0f))
            {
                float slowdown = Mathf.Min(
                    characterRigidbody.velocity.sqrMagnitude, friction);

            }
        }
    }

    private void CapSpeed()
    {
        // get difference from max speed
        float speedDiff = moveSpeed -
            Mathf.Abs(characterRigidbody.velocity.sqrMagnitude);
        if (speedDiff <= 0)
        {
            // calculate direction
            Vector3 dir = new Vector3(
                Mathf.Sign(characterRigidbody.velocity.x),
                Mathf.Sign(characterRigidbody.velocity.y),
                Mathf.Sign(characterRigidbody.velocity.z));
            // kill excess speed
            characterRigidbody.AddForce(speedDiff * characterRigidbody.mass *
            -dir, ForceMode.Impulse);
        }
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
            transform.rotation =
                Quaternion.LookRotation(cameraAdjustedInputDirection);
        }
    }

    override public void Jump()
    {
        throw new System.NotImplementedException();
    }

    override public void JumpCanceled()
    {
        throw new System.NotImplementedException();
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
        // move
        MoveCharacter();

        // update rotation
        RotateCharacter();

        // cap movement speed
        CapSpeed();
    }
    #endregion
}
