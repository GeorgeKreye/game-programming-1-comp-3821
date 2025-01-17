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

    [Tooltip("The character model")]
    [SerializeField] private GameObject characterModel;

    [Tooltip("The death ragdoll to spawn")]
    [SerializeField] private GameObject deathRagdoll;

    [Tooltip("The audio source attached to this character")]
    [SerializeField] private AudioSource characterAudioSource;

    [Tooltip("The interact manager attached to this character")]
    [SerializeField] private CharacterInteractManager interactManager;

    /// <summary>
    /// The active gameManager instance
    /// </summary>
    private GameManager gameManager;

    private void SubscribeInputActions()
    {
        input.Player.Move.started += MoveActionPerformed;
        input.Player.Move.canceled += MoveActionCancelled;
        input.Player.Jump.started += JumpActionPerformed;
        input.Player.Jump.canceled += JumpActionCancelled;
        input.Player.Camera.performed += CameraActionPerformed;
        input.Player.Interact.performed += InteractActionPerformed;
        input.Player.Pause.performed += PauseActionPerformed;
        input.UI.Unpause.performed += UnpauseActionPerformed;
    }

    private void UnsubscribeInputActions()
    {
        input.Player.Move.started -= MoveActionPerformed;
        input.Player.Move.canceled -= MoveActionCancelled;
        input.Player.Jump.started -= JumpActionPerformed;
        input.Player.Jump.canceled -= JumpActionCancelled;
        input.Player.Camera.performed -= CameraActionPerformed;
        input.Player.Interact.performed -= InteractActionPerformed;
        input.Player.Pause.performed -= PauseActionPerformed;
        input.UI.Unpause.performed -= UnpauseActionPerformed;
    }

    void Awake()
    {
        // get input
        input = new CharacterInput();

        // listen for input actions
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

        // get game manager
        gameManager = GameManager.Instance;

        // listen for game over
        gameManager.OnGameOver.AddListener(OnGameOver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        // stop listening for input actions
        UnsubscribeInputActions();

        // stop listening for game over
        gameManager.OnGameOver.RemoveListener(OnGameOver);
    }

    #region Input Actions
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

    private void CameraActionPerformed(InputAction.CallbackContext context)
    {
        // turn off old camera
        cameras[currentCamera].SetActive(false);

        // turn on new camera
        currentCamera = (currentCamera + 1) % cameras.Length;
        cameras[currentCamera].SetActive(true);
    }

    private void InteractActionPerformed(InputAction.CallbackContext context)
    {
        // tell interact manager to interact
        interactManager.Interact();
    }

    private void PauseActionPerformed(InputAction.CallbackContext context)
    {
        // failsafe to prevent unneeded state changes
        if (gameManager.CurrentPauseState == GameManager.PauseState.Playing)
        {
            // tell game manager to pause
            gameManager.PauseGame();

            // pause sound
            characterAudioSource.Pause();
        }
    }

    private void UnpauseActionPerformed(InputAction.CallbackContext context)
    {
        // failsafe to prevent unneeded state changes
        if (gameManager.CurrentPauseState == GameManager.PauseState.Paused)
        {
            // tell game manager to resume
            gameManager.ResumeGame();

            // resume sound
            characterAudioSource.UnPause();
        }
    }
    #endregion

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

    /// <summary>
    /// Behavior on game over
    /// </summary>
    private void OnGameOver()
    {
        // spawn ragdoll
        Instantiate(deathRagdoll, transform.position, transform.rotation);

        // disable player
        characterModel.SetActive(false);
        UnsubscribeInputActions();
    }
}

