using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject RPSLogo;
    public GameObject PlayButton;
    public GameObject SettingsButton;
    public GameObject CreditsButton;
    public GameObject ExitGameButton;


    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        Debug.Log("Going to Main Menu.");
        RPSLogo.SetActive(true);
        PlayButton.SetActive(true);
        SettingsButton.SetActive(true);
        CreditsButton.SetActive(true);
        ExitGameButton.SetActive(true);
    }

    public void Play()
    {
        Debug.Log("Singleplayer or Multiplayer?");
        SceneManager.LoadScene(1);

    }

    public void Settings()
    {
        Debug.Log("Going to Settings Menu.");
    }

    public void Credits()
    {
        Debug.Log("Going to Credits.");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
