using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Compound decision that acts as an 'or' operation in AI logic
/// </summary>
public class SelectionDecision : Decision
{
    [Tooltip("The decisions used in the compound decision")]
    [SerializeField] private Decision[] decisions;

    // Performs decision logic
    public override bool Decide(AIStateController controller)
    {
        // Return false if decision list is empty
        if (decisions.Length == 0)
        {
            return false;
        }

        // Loop through decisions
        foreach (Decision decision in decisions)
        {
            // Return true if any decision is true
            if (decision.Decide(controller))
            {
                return true;
            }
        }
        return false; // If loop is completed, no decisions are true
    }
}
