using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Compound decision that acts as an 'and' in AI logic
/// </summary>
[CreateAssetMenu(menuName = "Pluggable AI/Decisions/Compound/Sequence",
    fileName = "Sequence")]
public class SequenceDecision : Decision
{
    [Tooltip("The decisions to use")]
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
            // Return false if any decision is false
            if (!decision.Decide(controller))
            {
                return false;
            }
        }

        // If loop is completed, all decisions returned true
        return true;
    }
}
