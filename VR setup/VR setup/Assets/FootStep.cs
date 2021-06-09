using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public float footStepVolume;
    public Sound sound;

    public void PlayFootStepSoundLeft()
    {
        SoundManager.current.PlaySound(sound, leftFoot.position, footStepVolume);
    }

    public void PlayFootStepSoundRight()
    {
        SoundManager.current.PlaySound(sound, rightFoot.position, footStepVolume);
    }
}
