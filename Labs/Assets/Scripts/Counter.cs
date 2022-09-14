using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour    
{
    // Counter variable
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Unused
    }

    // Update is called once per frame
    void Update()
    {
        // Increment count and print new count to console
        count++;
        Debug.Log(count);
    }
}
