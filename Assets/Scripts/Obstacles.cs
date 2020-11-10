using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameplayManager GameplayManager;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 PlayerSide = collision.transform.position - gameObject.transform.position;

            if (GameplayManager.PlayerTurn == 1)
            {
                switch (PlayerSide)
                {
                    case Vector2 side when side.Equals(Vector2.up):
                        Player1.GetComponent<Player>().CanMoveInDirections[0] = false;
                        break;
                    case Vector2 side when side.Equals(Vector2.left):
                        Player1.GetComponent<Player>().CanMoveInDirections[1] = false;
                        break;
                    case Vector2 side when side.Equals(Vector2.down):
                        Player1.GetComponent<Player>().CanMoveInDirections[2] = false;
                        break;
                    case Vector2 side when side.Equals(Vector2.right):
                        Player1.GetComponent<Player>().CanMoveInDirections[3] = false;
                        break;
                    default:
                        break;
                }
            }

            if (GameplayManager.PlayerTurn == 2)
            {
                switch (PlayerSide)
                {
                    case Vector2 side when side.Equals(Vector2.up):
                        Player2.GetComponent<Player>().CanMoveInDirections[0] = false;
                        break;
                    case Vector2 side when side.Equals(Vector2.left):
                        Player2.GetComponent<Player>().CanMoveInDirections[1] = false;
                        break;
                    case Vector2 side when side.Equals(Vector2.down):
                        Player2.GetComponent<Player>().CanMoveInDirections[2] = false;
                        break;
                    case Vector2 side when side.Equals(Vector2.right):
                        Player2.GetComponent<Player>().CanMoveInDirections[3] = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
