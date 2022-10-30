using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    [Header("Player Input")]
    [Tooltip("The movmement input from the player (read-only)")]
    [SerializeField] private Vector2 movementInput;

    // reference to our input actions object
    private CharacterInput input;

    [Tooltip("The movement input that's aligned with the camera direction " +
        "(read-only)")]
    [SerializeField] private Vector3 cameraAdjustedInputDirection;

    [Header("Camera")]
    [Tooltip("The possible Virtual Cameras to use")]
    [SerializeField] private GameObject[] cameras;
    [Tooltip("The index of the Virtual Camera to start with; defaults to 0")]
    [SerializeField] private int startingCamera = 0;

    /// <summary>
    /// The index of the current camera
    /// </summary>
    private int currentCamera;


    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraOrientation;

    [Header("Component/Object referece")]
    [Tooltip("The BaseMovement component to send movement input to")]
    [SerializeField] private BaseMovement characterMovement;

    private void SubscribeInputActions()
    {
        input.Player.Move.started += MoveActionPerformed;
        input.Player.Move.canceled += MoveActionCancelled;
        input.Player.Jump.started += JumpActionPerformed;
        input.Player.Jump.canceled += JumpActionCancelled;
        input.Player.Camera.performed += CameraActionPerformed;
    }

    private void UnsubscribeInputActions()
    {
        input.Player.Move.started -= MoveActionPerformed;
        input.Player.Move.canceled -= MoveActionCancelled;
        input.Player.Jump.started -= JumpActionPerformed;
        input.Player.Jump.canceled -= JumpActionCancelled;
        input.Player.Camera.performed -= CameraActionPerformed;
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
        // deactivate cameras
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[currentCamera].SetActive(false);
        }

        // activate initial camera
        currentCamera = startingCamera;
        cameras[currentCamera].SetActive(true);
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

        Debug.Log("Movement: " + movementInput);

        // get camera relative movement
        CalculateCameraRelativeInput();

        Debug.Log("Movement: " + cameraAdjustedInputDirection);

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

    private void CameraActionPerformed(InputAction.CallbackContext context)
    {
        // turn off old camera
        cameras[currentCamera].SetActive(false);

        // turn on new camera
        currentCamera = (currentCamera + 1) % cameras.Length;
        cameras[currentCamera].SetActive(true);
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

