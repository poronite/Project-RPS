using System;
using UnityEngine.Audio;
using UnityEngine;

//Used this tutorial for reference: https://www.youtube.com/watch?v=6OT43pvUyfY

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioInstance = null;

    public AudioClips[] Clips;

    public AudioMixerGroup SFX;
    public AudioMixerGroup Music;

    private void Awake()
    {
        if (AudioInstance == null)
        {
            AudioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        //set up the audio clips from the values in the inspector/AudioClips.cs
        foreach (AudioClips clip in Clips)
        {
            clip.Source = gameObject.AddComponent<AudioSource>();

            clip.Source.clip = clip.Clip;
            clip.Source.volume = clip.Volume;
            clip.Source.loop = clip.Loop;

            //define audio mixer so that the volume can be changed through settings
            switch (clip.MixerType)
            {
                case "SFX":
                    clip.Source.outputAudioMixerGroup = SFX;
                    break;
                case "Music":
                    clip.Source.outputAudioMixerGroup = Music;
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayClip(string name)
    {
        //find audio clip
        AudioClips ClipReady = Array.Find(Clips, clip => clip.Name == name);

        //In case name of the clip is misspelled
        if (ClipReady == null)
        {
            Debug.Log("Clip not found.");
            return;
        }

        ClipReady.Source.Play();
    }

    public void PlayMusic()
    {
        foreach (AudioClips clip in Clips)
        {
            if (clip.MixerType == "Music")
            {
                clip.Source.Stop();
            }
        }

        switch (TransferMap.TransferMapInst.Map)
        {
            case 0:
                PlayClip("MenuTheme");
                break;
            case 1:
                PlayClip("Map1Theme");
                break;
            case 2:
                PlayClip("Map2Theme");
                break;
            default:
                break;
        }
    }
}
