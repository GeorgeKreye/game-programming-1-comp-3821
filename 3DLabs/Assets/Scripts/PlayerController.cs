using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Player Input")]
    [Tooltip("The movmement input from the player")]
    [SerializeField] private Vector2 movementInput;

    // reference to our input actions object
    private PlayerInputActions playerInputActions;

    [Tooltip("The movement input that's aligned with the camera direction")]
    [SerializeField] private Vector3 cameraAdjustedInputDirection;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraOrientation;


    [Header("Component/Object referece")]
    [SerializeField] private BaseMovement characterMovement;
    [SerializeField] private PlayerInteractManager interactManager;

    private void SubscribeInputActions()
    {
        playerInputActions.Player.Move.started += MoveActionPerformed;
        playerInputActions.Player.Move.canceled += MoveActionCancelled;
        playerInputActions.Player.Jump.started += JumpActionPerformed;
        playerInputActions.Player.Jump.canceled += JumpActionCancelled;
        playerInputActions.Player.Interact.performed += InteractActionPerformed;
    }

    private void UnsubscribeInputActions()
    {
        playerInputActions.Player.Move.started -= MoveActionPerformed;
        playerInputActions.Player.Move.canceled -= MoveActionCancelled;
        playerInputActions.Player.Jump.started -= JumpActionPerformed;
        playerInputActions.Player.Jump.canceled -= JumpActionCancelled;
        playerInputActions.Player.Interact.performed -= InteractActionPerformed;
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();

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

    private void InteractActionPerformed(InputAction.CallbackContext context)
    {
        // interact
        interactManager.Interact();
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
            0,cameraAdjustedInputDirection.z);
    }

    private void SwitchActionMap(string mapName)
    {
        switch(mapName)
        {
            default:
            case "Player":
                playerInputActions.UI.Disable();
                playerInputActions.Player.Enable();
                break;
            case "UI":
                playerInputActions.Player.Disable();
                playerInputActions.UI.Enable();
                break;
        }
    }
}
