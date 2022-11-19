using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for the animation & delayed activation of a pressable button. Should
/// be used in conjuction with PressableButtonISO.
/// </summary>
public class PressableButtonO : MonoBehaviour
{
    #region Serialized Fields
    [Tooltip("The GameObject to animate")]
    [SerializeField] private Transform animationTarget;
    [Header("Parameters")]
    [Tooltip("The speed at which the press animation should move")]
    [SerializeField] private float pressSpeed = 1f;
    [Tooltip("The distance downward that the button should be pressed")]
    [SerializeField] private float pressDistance = 0.1f;
    #endregion

    #region Unity Event
    [Tooltip("Event for when the button is pressed; delayed to allow button " +
        "to be fully depressed")]
    public UnityEvent OnButtonPressed;
    #endregion

    #region Nonserialized Fields
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool playing = false;
    private bool reversed = false;
    #endregion

    /// <summary>
    /// Causes the pressing animation to play.
    /// </summary>
    public void Press()
    {
        // start animation
        playing = true;
        Debug.Log("Button pressed = " + playing);
    }

    #region Unity Functions
    void Awake()
    {
        // save starting position
        startPosition = animationTarget.position;

        // calculate end position
        endPosition = new Vector3(animationTarget.position.x,
            animationTarget.position.y - pressDistance,
            animationTarget.position.z);
    }

    void Update()
    {
        // animate via non-physics movement if playing
        if (playing)
        {
            // determine movement direction for this frame
            Vector3 target;
            if (!reversed)
            {
                // move down
                target = endPosition;
            }
            else
            {
                // move up
                target = startPosition;
            }

            // perform movement
            animationTarget.position = Vector3.MoveTowards(
                animationTarget.position, target, pressSpeed * Time.deltaTime);
            Debug.Log("Position: " + animationTarget.position);

            // reverse & invoke event at endpoint
            if (!reversed && Mathf.Approximately(Vector3.Distance(
                animationTarget.position, endPosition), 0f))
            {
                reversed = true;
                OnButtonPressed.Invoke();
                Debug.Log("Invoking event");
            }

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
}
