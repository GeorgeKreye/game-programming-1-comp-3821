using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerControl : MonoBehaviour
{

    public float moveSpeed; // Increment to move by per frame

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        // Get keyboard
        var keyboard = Keyboard.current;

        // Check if directionals were pressed and move accordingly;
        // allow canceling & combination of inputs
        if (keyboard != null && keyboard.wKey.isPressed) // Up
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y + moveSpeed);
        }
        if(keyboard != null && keyboard.aKey.isPressed) // Left
        {
            transform.position = new Vector3(transform.position.x -
                moveSpeed,transform.position.y);
        }
        if (keyboard != null && keyboard.sKey.isPressed) // Down
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y - moveSpeed);
        }
        if (keyboard != null && keyboard.dKey.isPressed) // Right
        {
            transform.position = new Vector3(transform.position.x +
                moveSpeed, transform.position.y);
        }
    }
}