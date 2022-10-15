using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SFXChanger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlaySFX(AudioClip clipToPlay)
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
