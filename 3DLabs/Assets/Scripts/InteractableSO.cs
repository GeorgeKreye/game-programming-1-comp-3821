using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableSO : ScriptableObject
{
    /// <summary>
    /// Behavior to perform on interaction
    /// </summary>
    public abstract void InteractBehavior(PlayerInteractManager manager,
        PlayerController controller);
}
