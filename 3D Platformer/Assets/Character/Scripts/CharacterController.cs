using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    [Header("Player Input")]
    [Tooltip("The movmement input from the player")]
    [SerializeField] private Vector2 movementInput;

    // reference to our input actions object
    private CharacterInput input;

    [Tooltip("The movement input that's aligned with the camera direction")]
    [SerializeField] private Vector3 cameraAdjustedInputDirection;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraOrientation;


    [Header("Component/Object referece")]
    [SerializeField] private BaseMovement characterMovement;

    private void SubscribeInputActions()
    {
        input.Player.Move.started += MoveActionPerformed;
        input.Player.Move.canceled += MoveActionCancelled;
        input.Player.Jump.started += JumpActionPerformed;
        input.Player.Jump.canceled += JumpActionCancelled;
    }

    private void UnsubscribeInputActions()
    {
        input.Player.Move.started -= MoveActionPerformed;
        input.Player.Move.canceled -= MoveActionCancelled;
        input.Player.Jump.started -= JumpActionPerformed;
        input.Player.Jump.canceled -= JumpActionCancelled;
    }

    void Awake()
    {
        input = new CharacterInput();

        SubscribeInputActions();

        // start with player action map
        SwitchActionMap("Player");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        UnsubscribeInputActions();
    }

    private void MoveActionPerformed(InputAction.CallbackContext context)
    {
        // get movement
        movementInput = context.ReadValue<Vector2>();

        // get camera relative movement
        CalculateCameraRelativeInput();

        // move player
        characterMovement.Move(cameraAdjustedInputDirection);
    }

    private void JumpActionPerformed(InputAction.CallbackContext context)
    {
        // jump
        characterMovement.Jump();
    }

    private void MoveActionCancelled(InputAction.CallbackContext context)
    {
        // cancel movement
        characterMovement.Move(Vector3.zero);
    }

    private void JumpActionCancelled(InputAction.CallbackContext context)
    {
        // cancel jump
        characterMovement.JumpCanceled();
    }

    private void CalculateCameraRelativeInput()
    {
        // calculate adjusted direction
        cameraAdjustedInputDirection =
            cameraOrientation.forward * movementInput.y +
            cameraOrientation.right * movementInput.x;

        // possibly normalize if vector is too big
        if (cameraAdjustedInputDirection.sqrMagnitude > 1f)
        {
            cameraAdjustedInputDirection =
                cameraAdjustedInputDirection.normalized;
        }

        // horizontalize
        cameraAdjustedInputDirection = new Vector3(
            cameraAdjustedInputDirection.x,
            0, cameraAdjustedInputDirection.z);
    }

    private void SwitchActionMap(string mapName)
    {
        switch (mapName)
        {
            default:
            case "Player":
                input.UI.Disable();
                input.Player.Enable();
                break;
            case "UI":
                input.Player.Disable();
                input.UI.Enable();
                break;
        }
    }
}

