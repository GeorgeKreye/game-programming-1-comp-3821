using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for enemy behavior
/// </summary>
public class Enemy : MonoBehaviour
{
    [Tooltip("The Rigidbody2D attached to this enemy")]
    [SerializeField] private Rigidbody2D body;
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
        // Get player control
        if (player == null)
        {
            Debug.LogError("No reference to player GameObject");
        }
        playerControl = player.GetComponent<LCControl>();
        if (playerControl == null)
        {
            Debug.LogError("Could not find player controller");
        }

        // Get game manager
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("Could not find active game manager");
        }
    }

    // Collision handling
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collison was with player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove life
            gameManager.RemoveLife();

            // Send player back to checkpoint
            playerControl.ToLastCheckpoint();
        }
    }
}
