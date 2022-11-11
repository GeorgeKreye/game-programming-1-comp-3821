using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InteractibleSO/RandomSound",
    fileName = "RandomSoundISO")]
public class RandomSoundISO : InteractableSO
{
    [Tooltip("List of audio clips to play at random")]
    [SerializeField] List<AudioClip> randomAudioClipList;

    public override void InteractBehavior(PlayerInteractManager manager,
        PlayerController controller)
    {
        // Check if there are sounds in list
        if (randomAudioClipList.Count > 0)
        {
            // Select random sound
            int randomIndex = Random.Range(0, randomAudioClipList.Count);

            // Play sound
            controller.gameObject.GetComponent<AudioSource>(
                ).PlayOneShot(randomAudioClipList[randomIndex]);
        }
    }
}
