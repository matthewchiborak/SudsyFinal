using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour {

    public AudioSource moveSound;
    public AudioSource winSound;
    public AudioSource resetSound;

    public void playMoveSound()
    {
        moveSound.Play();
    }

    public void playWinSound()
    {
        winSound.Play();
    }

    public void playResetSound()
    {
        resetSound.Play();
    }
}
