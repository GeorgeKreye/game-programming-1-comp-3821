using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for exit portal behavior
/// </summary>
public class ExitPortalScript : MonoBehaviour
{
    [Tooltip("The player GameObject to check for collision with")]
    [SerializeField] private GameObject player;
    [Tooltip("The name of the scene to switch to on collision")]
    [SerializeField] private string nextScene;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get game manager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("Could not find an active GameManager instance");
        }
    }

    // Collision handling
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player collided
        if (collision.gameObject.CompareTag("Player"))
        {
            // Switch scene
            gameManager.ChangeScene(nextScene);
        }
    }
}
