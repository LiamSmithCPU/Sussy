using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    FootStep,
    EngageFight1,
    EngageFight2,
    EngageFight3,
    DisengageFight1,
    DisengageFight2,
    DisengageFight3,
    Escape,
    Escaped
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
        audioMap.Add(Sound.EngageFight1, Resources.Load<AudioClip>("Sounds/Engage Fight Sounds/EngageFight 1"));
        audioMap.Add(Sound.EngageFight2, Resources.Load<AudioClip>("Sounds/Engage Fight Sounds/EngageFight 2"));
        audioMap.Add(Sound.EngageFight3, Resources.Load<AudioClip>("Sounds/Engage Fight Sounds/EngageFight 3"));

        audioMap.Add(Sound.DisengageFight1, Resources.Load<AudioClip>("Sounds/Disengage Fight Sounds/DisengageFight 1"));
        audioMap.Add(Sound.DisengageFight2, Resources.Load<AudioClip>("Sounds/Disengage Fight Sounds/DisengageFight 2"));
        audioMap.Add(Sound.DisengageFight3, Resources.Load<AudioClip>("Sounds/Disengage Fight Sounds/DisengageFight 3"));

        audioMap.Add(Sound.Escape, Resources.Load<AudioClip>("Sounds/New Sounds/Run Alarm Sound"));
        audioMap.Add(Sound.Escaped, Resources.Load<AudioClip>("Sounds/New Sounds/Game Win"));
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
        aSource.spatialBlend = 1;
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

    public void PlaySound(Sound sound, Vector3 position, float multiplier)
    {
        PlayClipAt(audioMap[sound], position, volume * multiplier);
    }
}
