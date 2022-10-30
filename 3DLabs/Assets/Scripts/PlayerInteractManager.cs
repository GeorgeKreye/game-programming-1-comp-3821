using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractManager : MonoBehaviour
{
    /// <summary>
    /// The list of interactable GameObjects currently being tracked
    /// </summary>
    List<GameObject> interactableObjects = new List<GameObject>();

    [Tooltip("The PlayerController being used to handle interaction")]
    [SerializeField] private PlayerController playerController;

    [Header("Events")]
    [Tooltip("Event called when there is at least 1 interactable object " +
        "nearby")]
    public UnityEvent OnInteractablesExist;
    [Tooltip("Event called when there are no longer any interactable " +
        "objects")]
    public UnityEvent OnInteractablesDoNotExist;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Adds the provided GameObject to the list of interactable GameObjects to
    /// track.
    /// </summary>
    /// <param name="obj">The GameObject to track</param>
    private void TrackObject(GameObject obj)
    {
        // Add to list of interactable objects being tracked
        interactableObjects.Add(obj);

        // Invoke OnInteractablesExist if list was previously empty
        if (interactableObjects.Count == 1)
        {
            OnInteractablesExist.Invoke();
        }
    }

    /// <summary>
    /// Removes the provided GameObject from the list of interactable
    /// GameObjects to track.
    /// </summary>
    /// <param name="obj">The GameObject to stop tracking</param>
    private void StopTrackingObject(GameObject obj)
    {
        // Make sure object is already in list
        if (interactableObjects.Contains(obj))
        {
            // Remove from list
            interactableObjects.Remove(obj);

            // Invoke OnInteractablesDoNotExist if list is now empty
            if(interactableObjects.Count == 0)
            {
                OnInteractablesDoNotExist.Invoke();
            }
        }
    }
}
