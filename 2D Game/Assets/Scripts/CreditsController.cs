using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsController : MonoBehaviour
{
    [Tooltip("The UI Document used by the credits menu")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("The UI used as the main menu")]
    [SerializeField] private GameObject mainMenu;

    /// <summary>
    /// The button that closes the credits menu
    /// </summary>
    private Button closeButton;

    /// <summary>
    /// The script that controls the main menu UI
    /// </summary>
    private MainMenuControl MMControl;


    // Start is called before the first frame update
    void Start()
    {
        // Get main menu control
        MMControl = mainMenu.GetComponent<MainMenuControl>();
        if (MMControl == null)
        {
            Debug.LogError("Could not find controller for main menu UI");
        }

        // Start hidden
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        // Show this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }

    void Close()
    {
        // Hide this UI
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;

        // Open main menu
        MMControl.Open();
    }
}
