using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int numberMovesLeft;
    private Vector2 playerPosition;
    public int PlayerID;
    public GameplayManager GameplayManager;

    //each token quantity and their location in the array:
    //{RockAttack, RockDefense, PaperAttack, PaperDefense, ScissorAttack, ScissorDefense}
    public int[] Tokens = new int[] {0, 0, 0, 0, 0, 0}; 

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

    //turn based movement that consumes 1 move each time the player moves 1 tile
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

        //send player moves left to GameplayManager for the UI
        GameplayManager.CurrentPlayerMovesLeftUI(numberMovesLeft);

        gameObject.transform.position = playerPosition;
    }
}
