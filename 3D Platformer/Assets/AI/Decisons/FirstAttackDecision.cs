using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decision for whether an AI hasn't already attacked
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/First Attack",
    fileName = "FirstAttack")]
public class FirstAttackDecision : Decision
{
    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // Return whether AI has attacked
        return !controller.hasAttacked;
    }
}
