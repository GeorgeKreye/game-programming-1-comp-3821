using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{

    public float rotSpeed; // Speed of rotation; direction det

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate by rotSpeed
        // Code based on best answer of
        // https://answers.unity.com/questions/580001/trying-to-rotate-a-2d-sprite.html
        transform.Rotate(Vector3.forward * rotSpeed);
    }
}
