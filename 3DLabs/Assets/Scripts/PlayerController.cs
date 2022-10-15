using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Input")]
    [Tooltip("The movmement input from the player")]
    [SerializeField] private Vector2 movementInput;

    [Tooltip("The movement input that's aligned with the camera direction")]
    [SerializeField] private Vector3 cameraAdjustedInputDirection;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraOrientation;


    [Header("Component/Object referece")]
    [SerializeField] private BaseMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
