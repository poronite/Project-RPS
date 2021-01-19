using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


//Used these tutorial for reference: 
//https://www.youtube.com/watch?v=YOaYQrN1oYQ
//https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/


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
        ResDropdown.value = PlayerPrefs.GetInt("Resolution", currentResIndex);
        FullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
    }

    public void SetResolution(int resIndex)
    {
        Resolution chosenRes = res[resIndex];

        Screen.SetResolution(chosenRes.width, chosenRes.height, Screen.fullScreen, chosenRes.refreshRate);
        PlayerPrefs.SetInt("Resolution", resIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(isFullscreen));
    }

    public void SetSFXVolume(float sfxVolume)
    {
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void ResolutionSetUp()
    {
        res = Screen.resolutions;

        ResDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        for (int i = 0; i < res.Length; i++)
        {
            string option = $"{res[i].width} x {res[i].height}, {res[i].refreshRate}hz";
            resOptions.Add(option);

            if (res[i].width == Screen.currentResolution.width && res[i].height == Screen.currentResolution.height && res[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResIndex = i;
            }
        }

        ResDropdown.AddOptions(resOptions);

        ResDropdown.value = currentResIndex;
        ResDropdown.RefreshShownValue();
    }
}
