using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    public GameplayManager GameplayManager;
    public AI AI;
    private int playercount = 0;

    private void Awake()
    {
        //Delegate ResetPlanks to ResetPlanksDelegate in the Gameplay Manager
        GameplayManager.instance.ResetPlanksDelegate += ResetPlanks;
    }

    public void DelegatePlanks()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("AI"))
        {
            playercount += 1;
            Debug.Log("Plank");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("AI"))
        {
            playercount -= 1;

            if (GameplayManager.Map2Planks > 1 && playercount == 0)
            {
                switch (gameObject.name)
                {
                    case "PlankLeft":
                        AI.Tilesmap[3, 6] = false;
                        AI.Tilesmap[3, 7] = false;
                        AI.Tilesmap[3, 8] = false;
                        break;
                    case "PlankMiddle":
                        AI.Tilesmap[9, 6] = false;
                        AI.Tilesmap[9, 7] = false;
                        AI.Tilesmap[9, 8] = false;
                        break;
                    case "PlankRight":
                        AI.Tilesmap[15, 6] = false;
                        AI.Tilesmap[15, 7] = false;
                        AI.Tilesmap[15, 8] = false;
                        break;
                    default:
                        break;
                }

                AI.MakeGrid();

                gameObject.SetActive(false);
                GameplayManager.Map2Planks -= 1;
            } 
        }
    }

    public void ResetPlanks()
    {
        gameObject.SetActive(true);
        playercount = 0;
    }
}
