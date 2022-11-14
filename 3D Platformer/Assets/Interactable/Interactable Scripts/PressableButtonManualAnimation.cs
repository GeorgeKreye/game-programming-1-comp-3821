using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for an interactable button that animates using transform. Used in
/// conjuction with a ModularInteractable instance which uses a CallScript ISO.
/// Manual transform also allows for delayed button event invoking.
/// </summary>
public class PressableButtonManualAnimation : MonoBehaviour, IInteractableScript
{
    #region Serialized Fields
    [Tooltip("Whether button interaction can only occur once")]
    [SerializeField] private bool oneUse = false;
    [Tooltip("Whether the button press event should be delayed for animation")]
    [SerializeField] private bool delayEvent = true;
    #region Animation
    [Header("Animation Parameters")]
    [Tooltip("The transform of the mesh to animate")]
    [SerializeField] private Transform animationTarget;
    [Tooltip("The speed at which the animation should move")]
    [SerializeField] private float pressSpeed = 1f;
    [Tooltip("The maximum displacement downward of the press animation")]
    [SerializeField] private float pressDistance = 0.1f;
    #endregion
    #endregion

    #region Event
    [Header("Events")]
    [Tooltip("Event invoked when the button is interacted with; can be " +
        "delayed to account for animation")]
    public UnityEvent ButtonPressed;
    #endregion

    #region Private fields
    /// <summary>
    /// Whether a one-use button has been used
    /// </summary>
    private bool used = false;
    /// <summary>
    /// Whether the button animation is playing
    /// </summary>
    private bool playing = false;
    /// <summary>
    /// Whether the button animation is reversed
    /// </summary>
    private bool reversed = false;
    /// <summary>
    /// The initial positon of the animated mesh
    /// </summary>
    private Vector3 startPosition;
    /// <summary>
    /// The fully displaced position of the animation
    /// </summary>
    private Vector3 endPosition;
    #endregion

    #region Unity Functions
    // Awake is called when loading the script instance
    void Awake()
    {
        // Get start position
        startPosition = animationTarget.position;

        // Calculate end position
        endPosition = new Vector3(animationTarget.position.x,
            animationTarget.position.y - pressDistance,
            animationTarget.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Animate if playing
        if (playing)
        {
            // Determine movement direction for this frame
            Vector3 target;
            if (!reversed)
            {
                // Move down
                target = endPosition;
            }
            else
            {
                // Move up
                target = startPosition;
            }

            // Perform movement
            animationTarget.position = Vector3.MoveTowards(
                animationTarget.position, target, pressSpeed * Time.deltaTime);

            // Reverse at endpoint
            if (!reversed && Mathf.Approximately(Vector3.Distance(
                animationTarget.position, endPosition), 0f))
            {
                reversed = true;

                // Invoke event if event was delayed
                if (delayEvent)
                {
                    ButtonPressed.Invoke();
                }
            }

            // Stop moving if animation has ended
            // stop moving if animation has ended
            if (Mathf.Approximately(Vector3.Distance(animationTarget.position,
                startPosition), 0f) && reversed)
            {
                playing = false;
                reversed = false;
            }
        }
    }
    #endregion

    // Perform interaciton behavior
    public void OnInteract(CharacterInteractManager manager,
        CharacterController controller)
    {
        // If one-use, don't do anything if already used; otherwise don't do
        // anything if already animating
        if (!used && !playing)
        {
            // Play animation if possible
            if (animationTarget != null)
            {
                playing = true;
            }

            // Invoke event if not delayed or no animation target
            if (!delayEvent || animationTarget == null)
            {
                ButtonPressed.Invoke();
            }

            // Prevent further use if one-use
            if (oneUse)
            {
                used = true;
            }
        }
    }
}
