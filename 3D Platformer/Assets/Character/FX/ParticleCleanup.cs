using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleanup : MonoBehaviour
{
    /// <summary>
    /// Deletes the GameObject when called
    /// </summary>
    public void Delete()
    {
        Destroy(gameObject);
    }
}
