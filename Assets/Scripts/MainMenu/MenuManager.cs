using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int Map = 0;

    //instance of Menu Manager
    public static MenuManager MenuInstance = null;

    //reference to the elements of the Main Menu
    public GameObject MainMenuElements;

    //references to the UI when choosing the map
    public GameObject MapElements;

    //references to the Settings Menu
    public GameObject SettingsMenu;

    //references to the Credits Screen
    public GameObject CreditsScreen;

    private void Awake()
    {
        MenuInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    //enable Main menu's objects and disable the rest
    public void MainMenu()
    {
        HideEveryElement();

        //display Main Menu
        MainMenuElements.SetActive(true);
    }

    //display choose map
    public void ChooseMap()
    {
        HideEveryElement();

        MapElements.SetActive(true);
    }

    //change scene and start game
    public void Play(int map)
    {
        MenuInstance.Map = map;

        SceneManager.LoadScene(1);
    }

    //disable Main menu's objects and enable Settings objects
    public void Settings()
    {
        HideEveryElement();
        SettingsMenu.SetActive(true);
    }

    //disable Main menu's objects and enable Credits objects
    public void Credits()
    {
        HideEveryElement();
        CreditsScreen.SetActive(true);
    }

    //exit the game
    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    //Hide every element everytime the player goes to another part of the menu
    public void HideEveryElement()
    {
        //Play ButtonPress Clip
        FindObjectOfType<AudioManager>().PlayClip("ButtonPress");

        MainMenuElements.SetActive(false);
        MapElements.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsScreen.SetActive(false);
    }
}
