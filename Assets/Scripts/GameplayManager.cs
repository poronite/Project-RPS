using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    //Instance of the Gameplay Manager used for CooldownDelegate
    public static GameplayManager instance = null;

    //1 = Player1's turn | 2 = Player2's turn
    public int PlayerTurn;

    //1 Round = 2 player turns
    private int numRounds = 0;
    //bool used to know when we can numRounds += 1
    private bool endRound = true;

    //variable used to easily change player moves on Unity inspector
    public int AssignNumberMoves = 0;

    //references to the players in the scene
    public GameObject Player1;
    public GameObject Player2;

    private GameObject attacker;
    private GameObject defender;

    //reference to the HUD script
    //created HUD script to declutter this one
    public HUD HUD;

    //reference to camera
    public Camera MainCamera;
    private Vector3 cameraPosition;

    //references to the special tiles
    public GameObject SpecialTile;

    public delegate void MultiDelegate();
    public MultiDelegate CooldownDelegate;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cameraPosition = Camera.main.transform.position;
        DetermineFirstToPlay();
    }

    //function to determine who goes first in a match
    private void DetermineFirstToPlay()
    {
        PlayerTurn = Random.Range(1, 3);
        ChangePlayerTurn(PlayerTurn);
        Debug.Log($"Player{PlayerTurn} goes first.");
    }

    //function to warn player how many moves are left
    public void MovesLeftUI(int movesleft)
    {
        HUD.MovesLeftHUD(movesleft);
    }

    //function that runs when EndTurnButton is clicked
    public void EndTurn()
    {
        switch (PlayerTurn)
        {
            case 1:
                ChangePlayerTurn(2);
                break;

            case 2:
                ChangePlayerTurn(1);
                break;

            default:
                break;
        }
    }

    //function that changes player turn
    public void ChangePlayerTurn(int PTurnTemp)
    {
        EndRound();

        PlayerTurn = PTurnTemp;

        if (PlayerTurn == 1)
        {
            Player1.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().enabled = false;
            Player1.GetComponent<Player>().NumberMovesLeft = AssignNumberMoves;
            Player1.GetComponent<Player>().hasAttackedthisTurn = false;
            cameraPosition.y = Player1.transform.position.y;
        }

        if (PlayerTurn == 2)
        {
            Player1.GetComponent<Player>().enabled = false;
            Player2.GetComponent<Player>().enabled = true;
            Player2.GetComponent<Player>().NumberMovesLeft = AssignNumberMoves;
            Player2.GetComponent<Player>().hasAttackedthisTurn = false;
            cameraPosition.y = Player2.transform.position.y;
        }

        Camera.main.transform.position = cameraPosition;

        HUD.ChangeTurnRoundHUD(PlayerTurn, numRounds);
    }

    //end round after both players play
    public void EndRound()
    {
        if (endRound == true)
        {
            numRounds += 1;
            endRound = false;
            DelegateCooldowns();
        }
        else
        {
            endRound = true;
        }
    }

    private void DelegateCooldowns()
    {
        CooldownDelegate();
    }

    //Attack functions

    //do you want to attack the enemy?
    public void AttackConfirmation(GameObject attackerTemp, GameObject defenderTemp)
    {
        attacker = attackerTemp;
        defender = defenderTemp;

        HUD.AttackConfirmationBox();
    }

    //begin battle!
    public void Attack()
    {
        //attack
        Debug.Log($"Player{attacker.GetComponent<Player>().PlayerID} attacks Player{defender.GetComponent<Player>().PlayerID}!");

        //attacker can't attack twice in their turn
        attacker.GetComponent<Player>().hasAttackedthisTurn = true;

        //activate buttons (temporary)
        HUD.EndTurnButton.GetComponent<Button>().enabled = true;
        HUD.MainMenuButton.GetComponent<Button>().enabled = true;
    }

    //function that runs when Menu Button in the scene is clicked
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
