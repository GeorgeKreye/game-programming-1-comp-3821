using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMethod : MonoBehaviour
{
    // Called by Enemy collision
    public void Kill()
    {
        // Print to console
        Debug.Log("Player killed");
    }
}
