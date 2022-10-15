using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
    [Tooltip("The UIDocument used by the game over UI")]
    [SerializeField] private UIDocument UIDoc;

    private Button menuButton;
    private Button quitButton;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get game manager
        gameManager = GameManager.Instance;

        // Get buttons
        VisualElement root = UIDoc.rootVisualElement;
        menuButton = root.Q<Button>("MenuButton");
        quitButton = root.Q<Button>("QuitButton");
        menuButton.clicked += MenuButtonClicked;
        quitButton.clicked += QuitButtonClicked;
    }

    void OnDestroy()
    {
        menuButton.clicked -= MenuButtonClicked;
        quitButton.clicked -= QuitButtonClicked;
    }


    void MenuButtonClicked()
    {
        // go to main menu
        gameManager.ChangeScene("MainMenu");
    }

    void QuitButtonClicked()
    {
        // quit
        Application.Quit();
    }
}
