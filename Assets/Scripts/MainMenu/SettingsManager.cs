using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


//Used these tutorials for reference: 
//https://www.youtube.com/watch?v=YOaYQrN1oYQ
//https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
//https://answers.unity.com/questions/1463609/screenresolutions-returning-duplicates.html


public class SettingsManager : MonoBehaviour
{
    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;

    public Dropdown ResDropdown;
    public Toggle FullscreenToggle;
    public Slider SFXSlider;
    public Slider MusicSlider;

    Resolution[] res;
    private int currentResIndex = 0;

    private void Start()
    {
        ResolutionSetUp();

        //get values from preferences, else get default values
        //the resolution value is in ResolutionSetUp()
        FullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
    }

    //set and save resolution based on the resolution in the chosen index
    public void SetResolution(int resIndex)
    {
        Resolution chosenRes = res[resIndex];

        Screen.SetResolution(chosenRes.width, chosenRes.height, Screen.fullScreen, Screen.currentResolution.refreshRate);
        PlayerPrefs.SetInt("Resolution", resIndex);
        Debug.Log(PlayerPrefs.GetInt("Resolution"));
    }

    //set and save fullscreen based on whether the toggle is on or off
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(isFullscreen));
    }

    //set and save SFX volume based on the value from the slider
    public void SetSFXVolume(float sfxVolume)
    {
        //the Log10(sfxVolume) * 20 is to guarantee that the sound doesn't go mute in the lower half of the slider
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20); 
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    //same thing as SetSFXVolume
    public void SetMusicVolume(float musicVolume)
    {
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void ResolutionSetUp()
    {
        //gets every resolution without getting duplicates because of the refresh rate
        res = Screen.resolutions.Where(resolution => resolution.refreshRate == Screen.currentResolution.refreshRate).ToArray();

        ResDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        for (int i = 0; i < res.Length; i++)
        {
            string option = $"{res[i].width} x {res[i].height}";
            resOptions.Add(option);

            if (res[i].width == Screen.currentResolution.width && res[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i; //this is for the dropdown to know what resolution to show by default
            }
        }

        ResDropdown.AddOptions(resOptions);

        ResDropdown.value = PlayerPrefs.GetInt("Resolution", currentResIndex);

        ResDropdown.RefreshShownValue();
    }
}
