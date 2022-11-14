using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Script used to control the HUD
/// </summary>
public class HUDController : MonoBehaviour
{
    [Tooltip("The UI Document used for the HUD")]
    [SerializeField] private UIDocument UIDoc;

    #region Private fields
    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;
    /// <summary>
    /// Container used for displaying health
    /// </summary>
    private VisualElement healthContainer;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;

        // Get UI elements
        VisualElement root = UIDoc.rootVisualElement;
        healthContainer = root.Q<VisualElement>("health");

        // Subscribe to Unity and UI events
        SubscribeToEvents();
    }

    // OnDestroy is called before this script instance is removed
    void OnDestroy()
    {
        // Unsubscribe from Unity and UI events
        UnsubscribeFromEvents();
    }
    #endregion

    #region Event subscription
    /// <summary>
    /// Subscribes this script instance to Unity & UI events
    /// </summary>
    private void SubscribeToEvents()
    {
        // GameManager events
        gameManager.OnHealthChanged.AddListener(OnHealthUpdate);
    }

    /// <summary>
    /// Unsubscribes this script instance from Unity & UI events
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        // GameManager events
        gameManager.OnHealthChanged.RemoveListener(OnHealthUpdate);
    }
    #endregion

    #region Event handling
    private void OnHealthUpdate()
    {

    }
    #endregion
}
