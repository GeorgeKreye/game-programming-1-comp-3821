using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRetargeter : MonoBehaviour
{
    #region Serialized Fields
    [Tooltip("Positions of target GameObjects to cycle through")]
    [SerializeField] private Transform[] targets;

    [Header("Parameters")]
    [Tooltip("The speed to move at between targets")]
    [SerializeField] private float moveSpeed = 0.25f;
    #endregion

    /// <summary>
    /// Input for changing targets
    /// </summary>
    private PlayerInputActions input;

    /// <summary>
    /// The index of the current target GameObject
    /// </summary>
    private int currentTarget;

    #region Unity Functions
    void Awake()
    {
        input = new PlayerInputActions();
        SubscribeInputActions();
        SwitchActionMap("Player");
    }

    void Start()
    {
        if (targets.Length < 1)
        {
            Debug.LogError("No targets found");
        }
        transform.position = targets[0].position;
        currentTarget = 0;
    }

    void FixedUpdate()
    {
        MoveToTarget();
    }

    void OnDestroy()
    {
        UnsubscribeInputActions();
    }
    #endregion

    #region Unity Events
    private void SubscribeInputActions()
    {
        input.Player.Move.started += ChangeTarget;
        input.Player.Move.canceled += ChangeTarget;
    }

    private void UnsubscribeInputActions()
    {
        input.Player.Move.started -= ChangeTarget;
        input.Player.Move.canceled -= ChangeTarget;
    }
    #endregion

    #region PlayerInputActions interaction
    private void ChangeTarget(InputAction.CallbackContext context)
    {
        // get input
        Vector2 input = context.ReadValue<Vector2>();

        // get left/right component
        float dir = input.x;

        // determine target change if applicable
        if (dir > 0)
        {
            currentTarget = (currentTarget + 1) % targets.Length;
        }
        else if (dir < 0)
        {
            currentTarget = (currentTarget - 1) % targets.Length;

            // correct for modulo behavior
            if (currentTarget == -1)
            {
                currentTarget = targets.Length - 1;
            }
        }
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
    #endregion

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            targets[currentTarget].position, moveSpeed);
    }
}
