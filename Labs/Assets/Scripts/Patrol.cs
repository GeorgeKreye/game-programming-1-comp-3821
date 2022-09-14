using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public Vector3 point1;
    private int step = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (step == 0) {
            transform.position = Vector3.MoveTowards(transform.position,
                point1,speed);
        }
    }
}