using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    public float speed;
    public Vector2 point1;
    public Vector2 point2;
    private bool movingBack;
    private Rigidbody2D rb2d;
    private float xIncrement;
    private float yIncrement;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        movingBack = false;
        xIncrement = (point2.x - point1.x) / (1 / speed);
        yIncrement = (point2.y - point1.y) / (1 / speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!movingBack)
        {
            //Move to new position
            rb2d.MovePosition(new Vector2(rb2d.position.x + xIncrement,
                rb2d.position.y + yIncrement));

            // Check if endpoint has been reached
            if (rb2d.position == point2)
            {
                movingBack = true;
            }
        }
        else
        {
            //Move to new position
            rb2d.MovePosition(new Vector2(rb2d.position.x - xIncrement,
                rb2d.position.y - yIncrement));


            // Check if endpoint has been reached
            if (rb2d.position == point1)
            {
                movingBack = false;
            }
        }
    }
}
