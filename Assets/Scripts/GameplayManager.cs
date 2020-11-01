using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public int PlayerTurn;
    private int numTurns = 0;
    private bool endTurn = true;
    public int AssignNumberMoves = 0;
    public GameObject Player1;
    public GameObject Player2;
    public Text WhosTurn;
    public Text NumTurns;

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

    public void ChangePlayerTurn(int PTurnTemp)
    {
        EndTurn();

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

        WhosTurn.text = $"Player {PlayerTurn}'s turn" ;
        NumTurns.text = "Number of Turns: " + numTurns;
    }

    public void EndTurn()
    {
        if (endTurn == true)
        {
            numTurns += 1;
            endTurn = false;
        }
        else
        {
            endTurn = true;
        }
    }
}
