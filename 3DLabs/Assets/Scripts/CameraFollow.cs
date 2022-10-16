using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The GameObject to follow")]
    [SerializeField] private GameObject target;

    [Tooltip("Offset from target to follow at")]
    [SerializeField] private Vector3 offset;

    private Rigidbody targetRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // get target rigidbody
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.position = target.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
