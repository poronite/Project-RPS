using UnityEngine.Audio;
using UnityEngine;

//Used this tutorial for reference: https://www.youtube.com/watch?v=6OT43pvUyfY

[System.Serializable]
public class AudioClips
{
    public string Name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume;

    public string MixerType;

    public bool Loop;

    [HideInInspector]
    public AudioSource Source;
}
