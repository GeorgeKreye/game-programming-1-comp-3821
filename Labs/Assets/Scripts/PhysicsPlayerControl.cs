using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsPlayerControl : MonoBehaviour
{
    public float forceStrength; // Strength of force, set in editor
    public float jumpForceMultiplier; // How much to multiply forceStrength
                                      // when jumping, set in editor
    private Rigidbody2D rb2d; // Rigidbody of object
    private SpriteRenderer rend;
    private Vector2 force; // Force vector
    private float fX; // X component of force
    private float fY; // Y component of force
    private bool faceRight;

    // Start is called before the first frame update
    void Start()
    {
        // Get rigidbody and create initial force vector
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rend = gameObject.GetComponent<SpriteRenderer>();
        force = new Vector2(0f, 0f);

        // start facing right
        faceRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset force components
        fX = 0f;
        fY = 0f;

        // Check if inputs are being pressed and update
        // movement components accordingly
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.wKey.isPressed &&
            rb2d.velocity.y == 0) // Jump
        {
            fY = forceStrength * jumpForceMultiplier;
        }
        if (keyboard != null && keyboard.aKey.isPressed) // Left
        {
            fX = -forceStrength;
        }
        else if (keyboard != null && keyboard.dKey.isPressed) // Right
        {
            fX = forceStrength;
        }

        // Update force vector
        force = new Vector2(fX, fY);
    }

    void FixedUpdate()
    {
        // Check if sprite should be flipped
        if (faceRight && force.x < 0)
        {
            faceRight = false;
            rend.flipX = true;

        }
        else if (!faceRight && force.x > 0)
        {
            faceRight = true;
            rend.flipX = false;
        }

        // Move rigidbody using current force vector
        rb2d.AddForce(force);
    }
}
