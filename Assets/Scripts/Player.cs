using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int NumberMovesLeft;
    private Vector2 playerPosition;
    private Vector2 tilePosition;
    public int PlayerID;
    public bool IsMoving = false;
    public bool CanAttack;
    public bool IsBattling = false;
    public bool HasAttackedThisTurn = false;
    public float speed = 10;
    public GameplayManager GameplayManager;
    public AI AI;
    public float AITimer = 0.0f;
    public int PathCount = 0;



    //each token quantity and their location in the array:
    //{RandomnessAttack, RandomnessDefense, PatienceAttack, PatienceDefense, StrategyAttack, StrategyDefense}
    public int[] Tokens = new int[] { 0, 0, 0, 0, 0, 0 };

    private string[] Colliders = new string[] {"Player", "AI", "Obstacle"};

    private void Start()
    {
        playerPosition = gameObject.transform.position;
        tilePosition = new Vector2(playerPosition.x, playerPosition.y);
    }

    void Update()
    {
        //can only play if it's their turn
        if (GameplayManager.PlayerTurn == PlayerID)
        {
            Actions();
        }
    }

    
    private void Actions() 
    {
        RaycastHit2D hit;

        playerPosition = gameObject.transform.position;

        float walk = speed * Time.deltaTime;

        AITimer += Time.deltaTime; //so that the AI doesn't move fast af

        //turn based movement that consumes 1 move each time the player moves 1 tile | AI also uses this function
        //up
        if ((Input.GetButtonDown("TileUp") || (gameObject.CompareTag("AI") && AITimer >= 1.0f && playerPosition.y < AI.Path[PathCount].y + 0.5f)) && IsMoving == false && IsBattling == false && NumberMovesLeft > 0) //y = 1; 
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(0, 1), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                ColliderIsPlayer(hit);
            }
            else
            {
                if (gameObject.CompareTag("AI"))
                {
                    if (PathCount < AI.Path.Count - 1)
                    {
                        PathCount++;
                    }
                    AITimer = 0.0f;
                }
                tilePosition = new Vector2(playerPosition.x, playerPosition.y + 1);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }
            
        }

        //left
        if ((Input.GetButtonDown("TileLeft") || (gameObject.CompareTag("AI") && AITimer >= 1.0f && playerPosition.x > AI.Path[PathCount].x + 0.5f)) && IsMoving == false && IsBattling == false && NumberMovesLeft > 0) //x = -1;
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(-1, 0), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                ColliderIsPlayer(hit);
            }
            else
            {
                if (gameObject.CompareTag("AI"))
                {
                    if (PathCount < AI.Path.Count - 1)
                    {
                        PathCount++;
                    }
                    AITimer = 0.0f;
                }
                tilePosition = new Vector2(playerPosition.x - 1, playerPosition.y);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }
        }

        //down
        if ((Input.GetButtonDown("TileDown") || (gameObject.CompareTag("AI") && AITimer >= 1.0f && playerPosition.y > AI.Path[PathCount].y + 0.5f)) && IsMoving == false && IsBattling == false && NumberMovesLeft > 0) //y = -1;
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(0, -1), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                ColliderIsPlayer(hit);
            }
            else
            {
                if (gameObject.CompareTag("AI"))
                {
                    if (PathCount < AI.Path.Count - 1)
                    {
                        PathCount++;
                    }
                    AITimer = 0.0f;
                }
                tilePosition = new Vector2(playerPosition.x, playerPosition.y - 1);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }   
        }

        //right
        if ((Input.GetButtonDown("TileRight") || (gameObject.CompareTag("AI") && AITimer >= 1.0f && playerPosition.x < AI.Path[PathCount].x + 0.5f)) && IsMoving == false && IsBattling == false && NumberMovesLeft > 0) //x = 1;
        {
            hit = Physics2D.Raycast(playerPosition, new Vector2(1, 0), 0.5f);
            if (hit.collider != null && IsCollider(hit.collider.gameObject))
            {
                ColliderIsPlayer(hit);
            }
            else
            {
                if (gameObject.CompareTag("AI"))
                {
                    if (PathCount < AI.Path.Count - 1)
                    {
                        PathCount++;
                    }
                    AITimer = 0.0f;
                }
                tilePosition = new Vector2(playerPosition.x + 1, playerPosition.y);
                NumberMovesLeft -= 1;
                IsMoving = true;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, tilePosition, walk);

        //player can't move while playerPosition isn't equal to targetPosition
        if (playerPosition == tilePosition)
        {
            IsMoving = false;
        }

        //send player moves left to GameplayManager for the UI
        GameplayManager.MovesLeftUI(NumberMovesLeft);

        //Press EndTurn button to end turn
        if (Input.GetButtonDown("EndTurn") && GameplayManager.PlayerTurn == 1 && IsMoving == false)
        {
            GameplayManager.EndTurn();
        }

        //AI
        //AI ends turn if it can't move or attack
        if (CompareTag("AI"))
        {
            if ((NumberMovesLeft == 0 && IsMoving == false) || HasAttackedThisTurn == true)
            {
                GameplayManager.EndTurn();
            }
        }
    }

    //function that verifies if the player has enough tokens to attack
    public void EnoughTokensToAttack()
    {
        if (Tokens[0] == 0 && Tokens[2] == 0 && Tokens[4] == 0)
        {
            CanAttack = false;
        }
        else
        {
            CanAttack = true;
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

    //Check if the Player is colliding with another player (for attack purposes)
    private void ColliderIsPlayer(RaycastHit2D hit)
    {
        if ((hit.collider.CompareTag("Player") || hit.collider.CompareTag("AI")) && HasAttackedThisTurn == false && CanAttack == true)
        {
            //attack
            Debug.Log("Player Attack");

            GameObject attacker = gameObject;
            GameObject defender = hit.collider.gameObject;

            GameplayManager.AttackConfirmation(attacker, defender);
        }
    }
}
