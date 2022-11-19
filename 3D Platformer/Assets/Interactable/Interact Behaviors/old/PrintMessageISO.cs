using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InteractibleSO/PrintMessageISO",
    fileName = "PrintMessage")]
public class PrintMessageISO : InteractableSO
{
    [Tooltip("Message to print upon interaction")]
    [SerializeField] private string message;

    public override void InteractBehavior(CharacterInteractManager manager,
        CharacterController controller)
    {
        Debug.Log("PrintMessage ISO: " + message);
    }
}
