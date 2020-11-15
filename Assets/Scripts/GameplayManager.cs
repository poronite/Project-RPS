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

    //variable that determines if attacker can attack (in case he doesn't have attack tokens)
    public bool CanAttack;

    //variable that represents the outcome through affinity
    public string AffinityOutcome;

    //variable who determines who is the winner
    public string Winner = "";

    //variable used to easily change player moves on Unity inspector
    public int AssignNumberMoves = 0;

    //references to the players in the scene
    public GameObject Player1;
    public GameObject Player2;

    //attacker defender and their tokens
    public GameObject Attacker;
    public int[] AttackerTokens = new int[6];
    public GameObject Defender;
    public int[] DefenderTokens = new int[6];

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
            Player1.GetComponent<Player>().NumberMovesLeft = AssignNumberMoves;
            Player1.GetComponent<Player>().hasAttackedthisTurn = false;
            Player1.GetComponent<Player>().HasAttacked = false;
            cameraPosition.y = Player1.transform.position.y;
        }

        if (PlayerTurn == 2)
        {
            Player2.GetComponent<Player>().NumberMovesLeft = AssignNumberMoves;
            Player2.GetComponent<Player>().hasAttackedthisTurn = false;
            Player2.GetComponent<Player>().HasAttacked = false;
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
        Attacker = attackerTemp;
        Defender = defenderTemp;

        AttackerTokens = Attacker.GetComponent<Player>().Tokens;
        DefenderTokens = Defender.GetComponent<Player>().Tokens;

        if (AttackerTokens[0] == 0 && AttackerTokens[2] == 0 && AttackerTokens[4] == 0)
        {
            CanAttack = false;
        }
        else
        {
            CanAttack = true;
        }

        //disable attacker and defender actions while attacker is choosing to attack
        Attacker.GetComponent<Player>().enabled = false;
        Defender.GetComponent<Player>().enabled = false;

        HUD.AttackConfirmationScreen();
    }

    //get players ready for battle!
    public void ReadyAttack()
    {
        //re-enable their components
        Attacker.GetComponent<Player>().enabled = true;
        Defender.GetComponent<Player>().enabled = true;

        //both are now battling
        Attacker.GetComponent<Player>().HasAttacked = true;
        Defender.GetComponent<Player>().HasAttacked = true;

        //attacker can't attack twice in their turn
        Attacker.GetComponent<Player>().hasAttackedthisTurn = true;
    }

    public void CheckTokenAvailability()
    {
        if (AttackerTokens[0] == 0)
        {
            HUD.AttackerRButton.interactable = false;
            HUD.AttackerRButton.image.color = Color.gray;
        }

        if (AttackerTokens[2] == 0)
        {
            HUD.AttackerPButton.interactable = false;
            HUD.AttackerPButton.image.color = Color.gray;
        }

        if (AttackerTokens[4] == 0)
        {
            HUD.AttackerSButton.interactable = false;
            HUD.AttackerSButton.image.color = Color.gray;
        }

        if (DefenderTokens[1] == 0)
        {
            HUD.DefenderRButton.interactable = false;
            HUD.DefenderRButton.image.color = Color.gray;
        }

        if (DefenderTokens[3] == 0)
        {
            HUD.DefenderPButton.interactable = false;
            HUD.DefenderPButton.image.color = Color.gray;
        }

        if (DefenderTokens[5] == 0)
        {
            HUD.DefenderSButton.interactable = false;
            HUD.DefenderSButton.image.color = Color.gray;
        }

        if (DefenderTokens[1] == 0 && DefenderTokens[3] == 0 && DefenderTokens[5] == 0)
        {
            Winner = Attacker.name;
            HUD.Outcome();
        }
    }

    public void AttackSystem(string attackerChoice, string defenderChoice)
    {
        if (attackerChoice == "Randomness")
        {
            AttackerTokens[0] -= 1;
            switch (defenderChoice)
            {
                case "Randomness": //Neutral
                    DefenderTokens[1] -= 1;
                    AffinityOutcome = "Neutral";
                    break;
                case "Patience": //Disadvantage
                    //Nothing happens
                    AffinityOutcome = "Disadvantage";
                    break;
                case "Strategy": //Advantage
                    DefenderTokens[5] -= 1;
                    AttackerTokens[4] += 1;
                    AffinityOutcome = "Advantage";
                    break;
                default:
                    break;
            }
        }

        if (attackerChoice == "Patience")
        {
            AttackerTokens[2] -= 1;
            switch (defenderChoice)
            {
                case "Randomness": //Advantage
                    DefenderTokens[1] -= 1;
                    AttackerTokens[0] += 1;
                    AffinityOutcome = "Advantage";
                    break;
                case "Patience": //Neutral
                    DefenderTokens[3] -= 1;
                    AffinityOutcome = "Neutral";
                    break;
                case "Strategy": //Disadvantage
                    //Nothing happens
                    AffinityOutcome = "Disadvantage";
                    break;
                default:
                    break;
            }
        }

        if (attackerChoice == "Strategy")
        {
            AttackerTokens[4] -= 1;
            switch (defenderChoice)
            {
                case "Randomness": //Disadvantage
                    //Nothing happens
                    AffinityOutcome = "Disadvantage";
                    break;
                case "Patience": //Advantage
                    DefenderTokens[3] -= 1;
                    AttackerTokens[2] += 1;
                    AffinityOutcome = "Advantage";
                    break;
                case "Strategy": //Neutral
                    DefenderTokens[5] -= 1;
                    AffinityOutcome = "Neutral";
                    break;
                default:
                    break;
            }
        }

        HUD.Outcome();
    }

    public void CancelAttack()
    {
        //re-enable their components in case attacker doesn't want to attack
        Attacker.GetComponent<Player>().enabled = true;
        Defender.GetComponent<Player>().enabled = true;
    }

    public void EndMatch()
    {
        if (Winner != null)
        {
            Application.Quit();
        }
    }

    //function that runs when Menu Button in the scene is clicked
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
