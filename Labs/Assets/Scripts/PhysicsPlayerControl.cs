using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsPlayerControl : MonoBehaviour
{
    public float forceStrength; // Strength of force, set in editor
    private Rigidbody2D rb2d; // Rigidbody of object
    private Vector2 force; // Force vector
    private float fX; // X component of force
    private float fY; // Y component of force

    // Start is called before the first frame update
    void Start()
    {
        // Get rigidbody and create initial force vector
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        force = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Reset force components
        fX = 0f;
        fY = 0f;

        // Check if inputs are being pressed and update
        // movement components accordingly; allow for
        // canceling and diagonal movement
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.wKey.isPressed) // Up
        {
            fY = forceStrength;
        }
        if (keyboard != null && keyboard.aKey.isPressed) // Left
        {
            fX = -forceStrength;
        }
        if (keyboard != null && keyboard.sKey.isPressed) // Down
        {
            fY = -forceStrength;
        }
        if (keyboard != null && keyboard.dKey.isPressed) // Right
        {
            fX = forceStrength;
        }

        // Update force vector
        force = new Vector2(fX, fY);
    }

    void FixedUpdate()
    {
        // Move rigidbody using current force vector
        rb2d.AddForce(force);
    }
}
