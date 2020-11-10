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
    public int[] Tokens = new int[] { 0, 0, 0, 0, 0, 0 };

    //each bool represents a direction and if the player can move or not in that direction
    //{up, left, down, right}
    public bool[] CanMoveInDirections = new bool[] { true, true, true, true };

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

        if (Input.GetButtonDown("TileUp") && CanMoveInDirections[0] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = 1; 
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y + 1);
            NumberMovesLeft -= 1;
        }

        if (Input.GetButtonDown("TileLeft") && CanMoveInDirections[1] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = -1;
        {
            targetPosition = new Vector2(playerPosition.x - 1, playerPosition.y);
            NumberMovesLeft -= 1;
        }

        if (Input.GetButtonDown("TileDown") && CanMoveInDirections[2] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = -1;
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y - 1);
            NumberMovesLeft -= 1;
        }

        if (Input.GetButtonDown("TileRight") && CanMoveInDirections[3] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = 1;
        {
            targetPosition = new Vector2(playerPosition.x + 1, playerPosition.y);
            NumberMovesLeft -= 1;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, walk);

        //send player moves left to GameplayManager for the UI
        GameplayManager.CurrentPlayerMovesLeftUI(NumberMovesLeft);
    }

    //Collider that prevents Player from moving to an obstacle or player
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Player"))
        {
            Vector2 PlayerSide = transform.position - collision.transform.position;

            switch (PlayerSide)
            {
                case Vector2 side when side.Equals(Vector2.down): //if Collider is below
                    CanMoveInDirections[0] = false;
                    break;
                case Vector2 side when side.Equals(Vector2.right): //if collider is on the right
                    CanMoveInDirections[1] = false;
                    break;
                case Vector2 side when side.Equals(Vector2.up): //if collider is above
                    CanMoveInDirections[2] = false;
                    break;
                case Vector2 side when side.Equals(Vector2.left): //if collider is on the left
                    CanMoveInDirections[3] = false;
                    break;
                default:
                    break;
            }
        }
    }

    //resets array so that the Player can walk on every direction after not being on the side of an obstacle or player
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Player"))
        {
            for (int i = 0; i < CanMoveInDirections.Length; i++)
            {
                CanMoveInDirections[i] = true;
            }
        }
    }
}
