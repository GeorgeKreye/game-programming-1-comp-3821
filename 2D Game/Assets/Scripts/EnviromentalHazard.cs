using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentalHazard : MonoBehaviour
{
    [Tooltip("The player GameObject")]
    [SerializeField] private GameObject player;

    /// <summary>
    /// The active GameManager instance
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// The player controller
    /// </summary>
    private LCControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure player can be found
        if (player == null)
        {
            Debug.LogError("No player GameObject found");
        }

        // Get game manager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("Could not find an active GameManager instance");
        }

        // Get player controller
        playerControl = player.GetComponent<LCControl>();
        if (playerControl == null)
        {
            Debug.LogError("Could not find a player controller");
        }
    }

    // Collision handling
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure collision was with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove life
            gameManager.RemoveLife();

            // Send player to last checkpoint
            playerControl.ToLastCheckpoint();
        }
    }
}
