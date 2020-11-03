using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public int PlayerTurn;

    private int numRounds = 0;
    private bool endRound = true;

    public int AssignNumberMoves = 0;

    public GameObject Player1;
    public GameObject Player2;

    public Text WhosTurn;
    public Text NumRounds;

    void Start()
    {
        DetermineFirstToPlay();
    }

    private void DetermineFirstToPlay()
    {
        PlayerTurn = Random.Range(1, 3);
        ChangePlayerTurn(PlayerTurn);
        Debug.Log($"Player{PlayerTurn} goes first.");
    }

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

    public void ChangePlayerTurn(int PTurnTemp)
    {
        EndRound();

        PlayerTurn = PTurnTemp;

        if (PlayerTurn == 1)
        {
            Player1.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().enabled = false;
            Player1.GetComponent<Player>().numberMovesLeft = AssignNumberMoves;
        }

        if (PlayerTurn == 2)
        {
            Player1.GetComponent<Player>().enabled = false;
            Player2.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().numberMovesLeft = AssignNumberMoves;
        }

        WhosTurn.text = $"Player {PlayerTurn}'s turn";
        NumRounds.text = "Round " + numRounds;
    }

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

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
