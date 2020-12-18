using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //reference to class that stores variables needed
    public TransferVariables TransferVariables;

    //reference to the elements of the Main Menu
    public GameObject MainMenuElements;

    //references to the UI when choosing singleplayer or multiplayer
    public GameObject GamemodeElements;

    //references to the UI when choosing the map
    public GameObject MapElements;


    //starts scene in the Main menu
    void Start()
    {
        MainMenu();
    }

    //enable Main menu's objects and disable the rest
    public void MainMenu()
    {
        HideEveryElement();
        
        //display Main Menu
        MainMenuElements.SetActive(true);
    }

    //display choose gamemode
    public void ChooseGamemode()
    {
        HideEveryElement();

        GamemodeElements.SetActive(true);
    }

    //display choose map
    public void ChooseMap(string gamemode)
    {
        TransferVariables.Gamemode = gamemode;

        HideEveryElement();

        MapElements.SetActive(true);
    }

    //change scene and start game
    public void Play(int map)
    {
        TransferVariables.Map = map;

        SceneManager.LoadScene(1);
    }

    //disable Main menu's objects and enable Settings objects
    public void Settings()
    {
        Debug.Log("Going to Settings Menu.");
        //HideEveryElement();
    }

    //disable Main menu's objects and enable Credits objects
    public void Credits()
    {
        Debug.Log("Going to Credits.");
        //HideEveryElement();
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
        MainMenuElements.SetActive(false);
        GamemodeElements.SetActive(false);
        MapElements.SetActive(false);
    }
}
