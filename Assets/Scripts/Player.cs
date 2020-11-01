using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int numberMovesLeft;
    private Vector2 playerPosition;
    public int PlayerID;
    public GameplayManager GameplayManager;


    void Start()
    {
        playerPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (GameplayManager.PlayerTurn == PlayerID && numberMovesLeft > 0)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (Input.GetKeyDown("w"))
        {
            playerPosition.y += 1;
            numberMovesLeft -= 1;
        }

        if (Input.GetKeyDown("a"))
        {
            playerPosition.x -= 1;
            numberMovesLeft -= 1;
        }

        if (Input.GetKeyDown("s"))
        {
            playerPosition.y -= 1;
            numberMovesLeft -= 1;
        }

        if (Input.GetKeyDown("d"))
        {
            playerPosition.x += 1;
            numberMovesLeft -= 1;
        }

        gameObject.transform.position = playerPosition;

        if (numberMovesLeft == 0)
        {
            switch (PlayerID)
            {
                case 1:
                    GameplayManager.ChangePlayerTurn(2);
                    break;

                case 2:
                    GameplayManager.ChangePlayerTurn(1);
                    break;

                default:
                    break;
            }
        }
    }
}
