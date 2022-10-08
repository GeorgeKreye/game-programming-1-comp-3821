using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab5Patrol : MonoBehaviour
{
    [Tooltip("The speed this GameObject will travel at")]
    [SerializeField]
    private float speed;
    [Tooltip("The points this GameObject will travel between")]
    [SerializeField]
    private List<Vector3> waypoints;
    [Tooltip("The duration in seconds to puase at each point")]
    [SerializeField]
    private float pauseTime;
    private Coroutine path;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        StartPatrolling();
    }

    public virtual void StartPatrolling()
    {
        path = StartCoroutine(PatrolWaypoints());
    }

    public virtual void StopPatrolling()
    {
        StopCoroutine(path);
    }

    public IEnumerator PatrolWaypoints()
    {
        // path forever, unless StopPatrolling is called
        while (true)
        {
            // iterate through all points
            foreach (Vector3 point in waypoints)
            {
                // move towards current point
                while (transform.position != point)
                {
                    // change sprite direction if moving
                    if (!Mathf.Approximately(Mathf.Abs(point.x) -
                        Mathf.Abs(transform.position.x), 0f))
                    {
                        if (point.x < transform.position.x) // moving left
                        {
                            sprite.flipX = false;
                        }
                        else // moving right
                        {
                            sprite.flipX = true;
                        }
                    }
                    // move
                    transform.position = Vector3.MoveTowards(transform.position, point, speed);
                    yield return null;
                }

                // pause between points
                yield return new WaitForSeconds(pauseTime);
            }
            yield return null;
        }
    }
}