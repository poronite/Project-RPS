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
    //{RandomnessAttack, RandomnessDefense, PatienceAttack, PatienceDefense, StrategyAttack, StrategyDefense}
    public int[] Tokens = new int[] { 0, 0, 0, 0, 0, 0 };

    private string[] Colliders = new string[] {"Player", "Obstacle"};

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
        Actions();
    }

    
    private void Actions()
    {
        RaycastHit2D hit;

        playerPosition = gameObject.transform.position;

        float walk = speed * Time.deltaTime;

        //turn based movement that consumes 1 move each time the player moves 1 tile
        if (Input.GetButtonDown("TileUp") && IsMoving == false && CanMoveInDirections[0] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = 1; 
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(0, 1), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //attack
                    Debug.Log("Player Attack");
                }
            }
            else
            {
                targetPosition = new Vector2(playerPosition.x, playerPosition.y + 1);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }
            
        }

        if (Input.GetButtonDown("TileLeft") && IsMoving == false && CanMoveInDirections[1] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = -1;
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(-1, 0), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //attack
                    Debug.Log("Player Attack");
                }
            }
            else
            {
                targetPosition = new Vector2(playerPosition.x - 1, playerPosition.y);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }
        }

        if (Input.GetButtonDown("TileDown") && IsMoving == false && CanMoveInDirections[2] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //y = -1;
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(0, -1), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //attack
                    Debug.Log("Player Attack");
                }
            }
            else
            {
                targetPosition = new Vector2(playerPosition.x, playerPosition.y - 1);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }   
        }

        if (Input.GetButtonDown("TileRight") && IsMoving == false && CanMoveInDirections[3] == true && (GameplayManager.PlayerTurn == PlayerID && NumberMovesLeft > 0)) //x = 1;
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(1, 0), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //attack
                    Debug.Log("Player Attack");
                }
            }
            else
            {
                targetPosition = new Vector2(playerPosition.x + 1, playerPosition.y);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, walk);

        //Player can't move while playerPosition isn't equal to targetPosition
        if (playerPosition == targetPosition)
        {
            IsMoving = false;
        }

        //send player moves left to GameplayManager for the UI
        GameplayManager.MovesLeftUI(NumberMovesLeft);

        //Press EndTurn button to end turn
        if (Input.GetButtonDown("EndTurn"))
        {
            GameplayManager.EndTurn();
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
