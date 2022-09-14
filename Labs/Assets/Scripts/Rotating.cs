using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{

    public float rotSpeed; // Speed of rotation

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotSpeed);
    }
}
