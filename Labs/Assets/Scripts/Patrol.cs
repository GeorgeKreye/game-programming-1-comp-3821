using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed; // Speed of movement
    public Vector3 point1; // First endpoint of patrol
    public Vector3 point2; // Second endpoint of patrol
    private int step = 0; // Whether to go to point 1 or 2


    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        if (step == 0) // Moving to point 1
        { 
            transform.position = Vector3.MoveTowards(transform.position,
                point1,speed);
            if (transform.position == point1) // Start moving to point 2
            {
                step = 1;
            }
        }
        else // Moving to point 2
        {
            transform.position = Vector3.MoveTowards(transform.position,
                point2, speed);
            if (transform.position == point2) // Start moving to point 1
            {
                step = 0;
            }
        }
    }
}