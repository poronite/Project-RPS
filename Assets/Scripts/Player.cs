using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int NumberMovesLeft;
    private Vector2 playerPosition;
    private Vector2 targetPosition;
    public int PlayerID;
    public float speed = 10;
    public GameplayManager GameplayManager;

    //each token quantity and their location in the array:
    //{RockAttack, RockDefense, PaperAttack, PaperDefense, ScissorAttack, ScissorDefense}
    public int[] Tokens = new int[] {0, 0, 0, 0, 0, 0};

    private void Start()
    {
        playerPosition = gameObject.transform.position;
        targetPosition = new Vector2(playerPosition.x, playerPosition.y);
    }

    void Update()
    {
        Movement();
    }

    //turn based movement that consumes 1 move each time the player moves 1 tile
    private void Movement() 
    {
        playerPosition = gameObject.transform.position;

        float walk = speed * Time.deltaTime;

        if (Input.GetKeyDown("w") && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = 1; 
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y + 1);
            NumberMovesLeft -= 1;
        }

        if (Input.GetKeyDown("a") && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = -1;
        {
            targetPosition = new Vector2(playerPosition.x - 1, playerPosition.y);
            NumberMovesLeft -= 1;
        }

        if (Input.GetKeyDown("s") && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = -1;
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y - 1);
            NumberMovesLeft -= 1;
        }

        if (Input.GetKeyDown("d") && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = 1;
        {
            targetPosition = new Vector2(playerPosition.x + 1, playerPosition.y);
            NumberMovesLeft -= 1;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, walk);

        //send player moves left to GameplayManager for the UI
        GameplayManager.CurrentPlayerMovesLeftUI(NumberMovesLeft);
    }
}
