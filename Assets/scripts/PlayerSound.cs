using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource caminarSource;
    public AudioClip ataqueAudio;
    public AudioClip saltarAudio;
    public AudioClip dolorAudio;
    public AudioClip caminarAudio;

    public void PlaySaltar()
    {
        audioSource.PlayOneShot(saltarAudio);
    }
    public void PlayDolor()
    {
        audioSource.PlayOneShot(dolorAudio);
    }
    public void PlayAtaque()
    {
        audioSource.PlayOneShot(ataqueAudio, 0.3f);
    }
}
