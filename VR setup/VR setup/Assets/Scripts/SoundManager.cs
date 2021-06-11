using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    FootStep
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;

    [Range(0, 1)]
    public float volume;

    public Dictionary<Sound, AudioClip> audioMap = new Dictionary<Sound, AudioClip>();

    private void Awake()
    {
        current = this;

        audioMap.Add(Sound.FootStep, Resources.Load<AudioClip>("Sounds/FootStep"));
    }

    AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        // set other aSource properties here, if desired
        aSource.volume = volume;
        aSource.dopplerLevel = 0;
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

    public void PlaySound(Sound sound, Vector3 position, float multiplier)
    {
        PlayClipAt(audioMap[sound], position, volume * multiplier);
    }
}
