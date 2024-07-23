using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Compound decision used as 'not' in AI logic
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/Compound/Not",
    fileName = "Not")]
public class NotDecision : Decision
{
    [Tooltip("The decision to invert")]
    [SerializeField] private Decision decision;

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // return true if decision is empty
        if (decision.Equals(null))
        {
            return true;
        }

        // return opposite of decision result
        return !decision.Decide(controller);
    }
}
