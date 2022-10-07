using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : MonoBehaviour
{
    [Tooltip("The UI Document used as the pause menu")]
    [SerializeField] private UIDocument UIDoc;

    // Start is called before the first frame update
    void Start()
    {
        // Hide pause UI
        OnPlayerUnpaused();
    }

    public void OnPlayerPaused()
    {
        // Make pause menu visible
        UIDoc.rootVisualElement.style.visibility = Visibility.Visible;
    }
    public void OnPlayerUnpaused()
    {
        // Make pause menu invisible
        UIDoc.rootVisualElement.style.visibility = Visibility.Hidden;
    }
}
