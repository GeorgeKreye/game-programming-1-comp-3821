using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab5Patrol : MonoBehaviour
{
    // Serialized fields
    [Tooltip("Endpoints for patrol")]
    [SerializeField]
    private Vector2[] points;
    [Tooltip("Number of frames to pause for at each endpoint")]
    [SerializeField]
    private int pauseTime;
    [Tooltip("Speed to move at between points")]
    [SerializeField]
    private float moveSpeed;

    // Private fields
    private int currentPoint;
    private int pauseCounter;

    // Called before first frame update
    void Start()
    {
        pauseCounter = pauseTime - 1;
        currentPoint = 0;
    }

    // Called every fixed framerate frame
    void FixedUpdate()
    {
        // Move if not pause paused
        if (pauseCounter == pauseTime - 1)
        {
            // Move closer to endpoint
            transform.position = Vector3.MoveTowards(transform.position,
                points[currentPoint], moveSpeed);

            // Check if current endpoint has been reached
            if (transform.position.Equals(points[currentPoint]))
            {
                // Update point to travel to next
                currentPoint = (currentPoint + 1) % points.Length;

                // Start pause
                pauseCounter = 0;
            }
        }
        else // Do nothing since paused
        {
            // Update pause counter
            pauseCounter++;
        }
    }
}
