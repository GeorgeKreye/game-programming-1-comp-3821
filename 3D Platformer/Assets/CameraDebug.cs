using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position +
            transform.forward, Color.red);
        Debug.DrawLine(transform.position, target.position, Color.blue);
    }
}
