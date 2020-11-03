using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //reference to the game logo in the menu
    public GameObject RPSLogo;

    //references to the buttons in the menu
    public GameObject PlayButton;
    public GameObject SettingsButton;
    public GameObject CreditsButton;
    public GameObject ExitGameButton;


    //starts scene in the Main menu
    void Start()
    {
        MainMenu();
    }

    //enable Main menu's objects and disable the rest
    public void MainMenu()
    {
        Debug.Log("Going to Main Menu.");
        RPSLogo.SetActive(true);
        PlayButton.SetActive(true);
        SettingsButton.SetActive(true);
        CreditsButton.SetActive(true);
        ExitGameButton.SetActive(true);
    }

    //for now it just loads play scene
    public void Play()
    {
        Debug.Log("Singleplayer or Multiplayer?");
        SceneManager.LoadScene(1);

    }

    //disable Main menu's objects and enable Settings objects
    public void Settings()
    {
        Debug.Log("Going to Settings Menu.");
    }

    //disable Main menu's objects and enable Credits objects
    public void Credits()
    {
        Debug.Log("Going to Credits.");
    }

    //exit the game
    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
