using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for detecting when the player has fallen off the map, returning them
/// to their starting position and applying a configurable amount of damage
/// </summary>
public class FallOut : MonoBehaviour
{
    [Tooltip("The amount of damage to apply when the player falls off the map")]
    [SerializeField] private int fallOutDamage = 3;
    [Tooltip("The transform of the player GameObject")]
    [SerializeField] private Transform player;

    /// <summary>
    /// The position the player started at
    /// </summary>
    private Vector3 startingPosition;

    /// <summary>
    /// The active Game Manager instance
    /// </summary>
    private GameManager gameManager;

    // Called before first frame
    private void Start()
    {
        // Get game manager
        gameManager = GameManager.Instance;

        // Get player starting position
        startingPosition = player.position;
    }

    // Trigger collision logic
    private void OnTriggerEnter(Collider other)
    {
        // Check if player
        if(other.CompareTag("Player"))
        {
            // Subtract damage
            if (fallOutDamage > 0)
            {
                gameManager.RemoveHealth(fallOutDamage);
            }

            // Return player to starting point
            player.position = startingPosition;
        }
    }
}
