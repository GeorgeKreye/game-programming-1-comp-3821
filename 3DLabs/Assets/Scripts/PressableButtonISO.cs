using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "InteractibleSO/PressableButton",
    fileName = "PressableButtonISO")]
public class PressableButtonISO : InteractableSO
{ 
    public override void InteractBehavior(PlayerInteractManager manager,
        PlayerController controller)
    {
        Debug.Log("Trying to press button");
        manager.GetActiveInteractable().GetComponent<PressableButton>().Press();
    }
}
