using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Check for collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Call kill() method if present
        KillMethod player = collision.GetComponent<KillMethod>();
        if (player != null)
        {
            player.Kill();
        }
    }
}
