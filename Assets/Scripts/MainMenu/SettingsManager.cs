using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Used this tutorial for reference: https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class SettingsManager : MonoBehaviour
{
    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;

    public Dropdown ResDropdown;

    Resolution[] res;

    private void Start()
    {
        ResolutionSetUp();
    }

    public void SetResolution(int resIndex)
    {
        Resolution chosenRes = res[resIndex];

        Screen.SetResolution(chosenRes.width, chosenRes.height, Screen.fullScreen, chosenRes.refreshRate);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetSFXVolume(float sfxVolume)
    {
        SFXMixer.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        MusicMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void ResolutionSetUp()
    {
        res = Screen.resolutions;

        ResDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        int currentResIndex = 0;

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
