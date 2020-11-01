using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public int PlayerTurn;
    public GameObject Player1;
    public GameObject Player2;

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
        PlayerTurn = PTurnTemp;

        if (PlayerTurn == 1)
        {
            Player1.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().enabled = false;
            Player1.GetComponent<Player>().numberMovesLeft = 2;
        }

        if (PlayerTurn == 2)
        {
            Player1.GetComponent<Player>().enabled = false;
            Player2.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().numberMovesLeft = 2;
        }

        Debug.Log($"It's Player{PlayerTurn}'s turn.");
    }
}
