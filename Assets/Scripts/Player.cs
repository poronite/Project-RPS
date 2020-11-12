using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int NumberMovesLeft;
    private Vector2 playerPosition;
    private Vector2 targetPosition;
    public int PlayerID;
    public bool IsMoving = false;
    public float speed = 10;
    public GameplayManager GameplayManager;

    //each token quantity and their location in the array:
    //{RockAttack, RockDefense, PaperAttack, PaperDefense, ScissorAttack, ScissorDefense}
    public int[] Tokens = new int[] { 0, 0, 0, 0, 0, 0 };

    private string[] Colliders = new string[] {"Player", "Obstacle" };

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
        

        if (Input.GetButtonDown("TileUp") && IsMoving == false && CanMoveInDirections[0] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = 1; 
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y + 1);
            NumberMovesLeft -= 1;
            IsMoving = true;
        }

        if (Input.GetButtonDown("TileLeft") && IsMoving == false && CanMoveInDirections[1] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = -1;
        {
            targetPosition = new Vector2(playerPosition.x - 1, playerPosition.y);
            NumberMovesLeft -= 1;
            IsMoving = true;
        }

        if (Input.GetButtonDown("TileDown") && IsMoving == false && CanMoveInDirections[2] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = -1;
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y - 1);
            NumberMovesLeft -= 1;
            IsMoving = true;
        }

        if (Input.GetButtonDown("TileRight") && IsMoving == false && CanMoveInDirections[3] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = 1;
        {
            targetPosition = new Vector2(playerPosition.x + 1, playerPosition.y);
            NumberMovesLeft -= 1;
            IsMoving = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, walk);

        if (playerPosition == targetPosition)
        {
            IsMoving = false;
        }

        //send player moves left to GameplayManager for the UI
        GameplayManager.CurrentPlayerMovesLeftUI(NumberMovesLeft);
    }

    //Collider that prevents Player from moving to an obstacle or player
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsCollider(gameObject))
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
            Debug.Log($"{collision.tag}");
        }
    }

    //resets array so that the Player can walk on every direction after not being on the side of an obstacle or player
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsCollider(gameObject))
        {
            for (int i = 0; i < CanMoveInDirections.Length; i++)
            {
                CanMoveInDirections[i] = true;
            }
            Debug.Log($"{collision.tag}");
        }
    }

    //Function that checks if the Player is colliding with an object with a tag listed in Colliders array
    private bool IsCollider(GameObject Player)
    {
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Player.CompareTag(Colliders[i]))
            {
                return true;
            }
        }

        return false;
    }

    
}
