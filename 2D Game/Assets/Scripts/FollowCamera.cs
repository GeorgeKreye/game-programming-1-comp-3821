using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Target Game Object to follow
    [Tooltip("The GameObject the camera should follow")]
    [SerializeField]
    private GameObject target;

    // Offset to follow at
    [Tooltip("The offset from the center of the GameObject that the camera " +
        "should follow at")]
    [SerializeField]
    private Vector2 offset;

    // Tracks target object's location
    private Transform targetLocation;

    private void Awake()
    {
        // Make sure target GameObject is assigned
        if (target == null)
        {
            Debug.LogError("No assigned target to follow");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get transform to track target object's location
        targetLocation = target.GetComponent<Transform>();

        // Set initial position
        transform.position = new Vector3(targetLocation.position.x + offset.x,
            targetLocation.position.y + offset.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Update position
        transform.position = new Vector3(targetLocation.position.x + offset.x,
            targetLocation.position.y + offset.y, transform.position.z);
    }
}
