using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for handling interaction
/// </summary>
public class CharacterInteractManager : MonoBehaviour
{
    /// <summary>
    /// The list of interactable GameObjects currently being tracked
    /// </summary>
#pragma warning disable IDE0044 // Stops misidentifying as only being read
    List<GameObject> interactableObjects = new List<GameObject>();
#pragma warning restore IDE0044

    [Tooltip("The CharacterController being used to handle interaction")]
    [SerializeField] private CharacterController playerController;

    #region Events
    [Header("Events")]
    [Tooltip("Event called when there is at least 1 interactable object " +
        "nearby")]
    public UnityEvent OnInteractablesExist;
    [Tooltip("Event called when there are no longer any interactable " +
        "objects")]
    public UnityEvent OnInteractablesDoNotExist;
    #endregion

    #region Tracking behavior
    /// <summary>
    /// Adds the provided GameObject to the list of interactable GameObjects to
    /// track
    /// </summary>
    /// <param name="obj">
    /// The GameObject to track
    /// </param>
    private void TrackObject(GameObject obj)
    {
        // Add to list of interactable objects being tracked
        interactableObjects.Add(obj);
        UnityEngine.Debug.Log("Tracking " + obj);

        // Invoke OnInteractablesExist if list was previously empty
        if (interactableObjects.Count == 1)
        {

            OnInteractablesExist.Invoke();
        }
    }

    /// <summary>
    /// Removes the provided GameObject from the list of interactable
    /// GameObjects to track
    /// </summary>
    /// <param name="obj">
    /// The GameObject to stop tracking
    /// </param>
    public void StopTrackingObject(GameObject obj)
    {
        // Make sure object is already in list
        if (interactableObjects.Contains(obj))
        {
            // Remove from list
            interactableObjects.Remove(obj);
            UnityEngine.Debug.Log("Stopped tracking " + obj);

            // Invoke OnInteractablesDoNotExist if list is now empty
            if (interactableObjects.Count == 0)
            {
                OnInteractablesDoNotExist.Invoke();
            }
        }
    }
    #endregion

    #region Trigger behavior
    private void OnTriggerEnter(Collider other)
    {
        // Check if object is interactable
        if (other.CompareTag("Interactable"))
        {
            // Track object
            TrackObject(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if object is interactable
        if (other.CompareTag("Interactable"))
        {
            // Stop tracking object if already being tracked
            StopTrackingObject(other.gameObject);
        }
    }
    #endregion

    /// <summary>
    /// Interacts with the first object to enter the Interact Manager's
    /// zone of interaction
    /// </summary>
    public void Interact()
    {
        // Make sure interactable objects exist
        if (interactableObjects.Count > 0)
        {
            // Make sure first element is not null
            while (interactableObjects[0] == null)
            {
                // Remove element if null
                StopTrackingObject(interactableObjects[0]);
            }

            // Interact with first object
            interactableObjects[0].GetComponent<IInteractable>().Interact(this,
                playerController);
            UnityEngine.Debug.Log("Interacted with " + interactableObjects[0]);
        }
    }

    /// <summary>
    /// Gets the interactable that is being interacted with. Should only be
    /// called when interacting; useful for interactable instances with
    /// specific behavior
    /// </summary>
    /// <returns>
    /// The GameObject that is being interacted with
    /// </returns>
    public GameObject GetActiveInteractable()
    {
        return interactableObjects[0];
    }
}
