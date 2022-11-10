using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pluggable AI/Decisions/Wait", fileName = "Wait")]
public class WaitDecision : Decision
{
    [Tooltip("The duration to wait for")]
    [SerializeField] private float duration;

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        return controller.timer >= duration;
    }
}
