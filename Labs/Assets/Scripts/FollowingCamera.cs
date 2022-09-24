using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [Tooltip("The Game Object the camera will follow")]
    public GameObject target;

    private Transform targetLocation;

    // Start is called before the first frame update
    void Start()
    {
        targetLocation = target.GetComponent<Transform>();
        if (targetLocation == null)
        {
            Debug.LogError("Couldn not find target object's location");
        }

        // Set initial position to match target position
        transform.position = new Vector3(targetLocation.position.x,
                targetLocation.position.y + 3, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if position needs to be updated
        if (transform.position.x != targetLocation.position.x ||
            transform.position.y + 3 != transform.position.y)
        {
            // Update position to match target position
            transform.position = new Vector3(targetLocation.position.x,
                targetLocation.position.y + 3,transform.position.z);
        }
    }
}
