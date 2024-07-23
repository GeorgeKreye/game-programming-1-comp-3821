using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controls the credits UI on the wind screen
/// </summary>
public class WinCreditsControl : MonoBehaviour
{
    [Tooltip("The UI Document used by the credits menu")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The UI used as the win menu")]
    [SerializeField] private GameObject winMenu;

    /// <summary>
    /// The button that closes the credits menu
    /// </summary>
    private Button closeButton;

    /// <summary>
    /// The script that controls the win menu UI
    /// </summary>
    private WinMenuController WMControl;


    // Start is called before the first frame update
    void Start()
    {
        // Get win menu control
#pragma warning disable UNT0026 // Handled by following if statement
        WMControl = winMenu.GetComponent<WinMenuController>();
#pragma warning restore UNT0026 
        if (WMControl == null)
        {
            Debug.LogError("Could not find controller for win menu UI");
        }

        // Start hidden
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;

        // Get close button
        closeButton = UIDoc.rootVisualElement.Q<Button>("CloseButton");

        // Set close button action
        closeButton.clicked += Close;
    }

    // OnDestroy is called before a script instance is destroyed
    void OnDestroy()
    {
        // Remove close button action
        closeButton.clicked -= Close;
    }

    /// <summary>
    /// Opens the credits UI
    /// </summary>
    public void Open()
    {
        // Show this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }

    /// <summary>
    /// Closes the credits UI
    /// </summary>
    void Close()
    {
        // Hide this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;

        // Open win menu
        WMControl.Open();
    }
}
