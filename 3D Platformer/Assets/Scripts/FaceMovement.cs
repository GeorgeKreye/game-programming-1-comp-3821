using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates model to face movement
/// </summary>
public class FaceMovement : MonoBehaviour
{
    [Tooltip("The transform of the model being rotated")]
    [SerializeField] private Transform model;
    [Tooltip("The speed at which to rotate")]
    [SerializeField] private float rotationSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        // Rotate to face movement direction
        model.forward = Vector3.Lerp(model.forward, transform.forward,
            rotationSpeed);
    }
}
