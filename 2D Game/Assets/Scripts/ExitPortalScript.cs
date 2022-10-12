using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortalScript : MonoBehaviour
{
    [Tooltip("The player GameObject to check for collision with")]
    [SerializeField] private GameObject player;
    [Tooltip("The BoxCollder2D of the exit portal, configured as trigger")]
    [SerializeField] private BoxCollider2D trigger;
    [Tooltip("The scene to switch to on collision")]
    [SerializeField] private string nextScene;

    /// <summary>
    /// The active Game Manager instance
    /// </summary>
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure trigger is present and configured
        if (trigger == null || trigger.isTrigger)
        {
            Debug.LogError("Could not find a BoxColldier2D trigger attached " +
                "to " + gameObject + "; make sure one is present and " +
                "properly configured");
        }

        // Get game manager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("Could not find an active GameManager instance");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision handling
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player collided
        if (collision.gameObject == player)
        {
            // Switch scene
            gameManager.ChangeScene(nextScene);
        }
    }
}
