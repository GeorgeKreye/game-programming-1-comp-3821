using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("The interactable scriptable object to use")]
    [SerializeField] private InteractableSO interactBehavior;

    public void Interact(PlayerInteractManager playerInteractManager,
        PlayerController playerController)
    {
        interactBehavior.InteractBehavior(playerInteractManager,
            playerController);
    }
}
