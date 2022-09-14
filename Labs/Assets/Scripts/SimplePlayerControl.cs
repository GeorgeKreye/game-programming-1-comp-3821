using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerControl : MonoBehaviour
{

    public float moveSpeed; // Increment to move by per frame
    private float currentH = 0f; // Current horizontal movement
    private float currentV = 0f; // Current vertical movement

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        // Check for player input and get any directions of movement
        currentH = Input.GetAxis("Horizontal");
        currentV = Input.GetAxis("Vertical");

        // Apply movement
        transform.position = new Vector3();
    }
}