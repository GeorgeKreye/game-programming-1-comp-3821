using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact(PlayerInteractManager playerInteractManager, PlayerController playerController);
}
