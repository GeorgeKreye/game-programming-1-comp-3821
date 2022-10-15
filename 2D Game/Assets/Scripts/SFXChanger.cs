using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SFXChanger : MonoBehaviour
{
    [SerializeField] private AudioClip victorySFX;
    [SerializeField] private AudioClip defeatSFX;

    [SerializeField] private AudioSource audioSource;

    private void Update()
    {
        Keyboard kb = Keyboard.current;

        // If we've pressed the 1 key, play the victorySFX
        if(kb.digit1Key.wasPressedThisFrame)
        {
            PlaySFX(victorySFX);
        }
        //If we've pressed the 2 key, play the defeatSFX
        else if (kb.digit2Key.wasPressedThisFrame)
        {
            PlaySFX(defeatSFX);
        }
    }

    private void PlaySFX(AudioClip clipToPlay)
    {
        //If we try to play the clip that is already playing, do not
        if (clipToPlay == audioSource.clip) return;

        //Stop playing a sound if a sound is being played
        audioSource.Stop();

        // Set what clip to play
        audioSource.clip = clipToPlay;
        
        //Play the sound
        audioSource.Play();
    }
}
