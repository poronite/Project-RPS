using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public int PlayerTurn;

    //1 Round = 2 player turns
    private int numRounds = 0;
    //bool used to know when we can numRounds += 1
    private bool endRound = true;

    //variable used to easily change player moves on Unity inspector
    public int AssignNumberMoves = 0;

    //references to the players in the scene
    public GameObject Player1;
    public GameObject Player2;

    //references to UI elements in the scene
    public Text WhosTurn;
    public Text CurrentPlayerMovesLeft;
    public Text NumRounds;
    public Button EndTurnButton;

    //reference to camera
    public Camera MainCamera;
    private Vector3 cameraPosition = new Vector3(0, 0, -10);


    void Start()
    {
        cameraPosition = Camera.main.transform.position;
        DetermineFirstToPlay();
    }

    //function to determine who goes first in a match
    private void DetermineFirstToPlay()
    {
        PlayerTurn = Random.Range(1, 3);
        ChangePlayerTurn(PlayerTurn);
        Debug.Log($"Player{PlayerTurn} goes first.");
    }

    //function to warn player how many moves are left
    public void CurrentPlayerMovesLeftUI(int movesleft)
    {
        if (movesleft > 0)
        {
            CurrentPlayerMovesLeft.text = $"{movesleft} moves left";
            EndTurnButton.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        else if (movesleft == 0)
        {
            CurrentPlayerMovesLeft.text = "No moves left";
            EndTurnButton.GetComponent<Image>().color = new Color(186, 253, 0); 
        }
    }

    //function that runs when EndTurnButton is clicked
    public void EndTurn()
    {
        switch (PlayerTurn)
        {
            case 1:
                ChangePlayerTurn(2);
                break;

            case 2:
                ChangePlayerTurn(1);
                break;

            default:
                break;
        }
    }

    //function that changes player turn
    public void ChangePlayerTurn(int PTurnTemp)
    {
        EndRound();

        PlayerTurn = PTurnTemp;

        if (PlayerTurn == 1)
        {
            Player1.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().enabled = false;
            Player1.GetComponent<Player>().numberMovesLeft = AssignNumberMoves;
            cameraPosition.y = Player1.transform.position.y;
        }

        if (PlayerTurn == 2)
        {
            Player1.GetComponent<Player>().enabled = false;
            Player2.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().numberMovesLeft = AssignNumberMoves;
            cameraPosition.y = Player2.transform.position.y;
        }

        Camera.main.transform.position = cameraPosition;

        WhosTurn.text = $"Player {PlayerTurn}'s turn";
        NumRounds.text = "Round " + numRounds;
    }

    //end round after both players play
    public void EndRound()
    {
        if (endRound == true)
        {
            numRounds += 1;
            endRound = false;
        }
        else
        {
            endRound = true;
        }
    }

    //function that runs when Menu Button in the scene is clicked
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
