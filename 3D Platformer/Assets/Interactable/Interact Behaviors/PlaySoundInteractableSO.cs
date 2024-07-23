using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum representing audio source options
/// </summary>
enum AudioSourceType
{
    PLAYER, INTERACTABLE
}

/// <summary>
/// Interactable SO that plays a sound
/// </summary>
[CreateAssetMenu (menuName = "Interactables/Play Sound",
    fileName = "PlaySoundISO")]
public class PlaySoundInteractableSO : InteractableSO
{
	[Tooltip("The audio clip to play")]
	[SerializeField] private AudioClip sound;

    [Tooltip("The audio source to play the sound from")]
    [SerializeField] private AudioSourceType source = AudioSourceType.PLAYER;

    // Performs behavior
    public override void InteractBehavior(CharacterInteractManager manager,
        CharacterController controller)
    {
        // Check source enum
        if (source == AudioSourceType.PLAYER)
        {
            // Play sound from player audio source
            controller.gameObject.GetComponent<AudioSource>().PlayOneShot(
                sound);
        }
        else if (source == AudioSourceType.INTERACTABLE)
        {
            // Play sound from interactable audio source
            manager.GetActiveInteractable().GetComponent<AudioSource>(
                ).PlayOneShot(sound);
        }
    }
}
