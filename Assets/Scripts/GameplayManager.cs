using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    //instance of the Gameplay Manager used for CooldownDelegate
    public static GameplayManager instance = null;

    //1 = Player1's turn | 2 = Player2's turn
    public int PlayerTurn;

    //map number
    public int Map;
    public GameObject Map1;
    public GameObject Map2;

    //1 Round = 2 player turns
    private int numRounds = 0;
    //bool used to know when we can numRounds += 1
    private bool endRound = true;

    //number of matches
    private int numberMatches = 0;
    //win match
    public bool MatchWin = false;

    //variable that represents the outcome through affinity
    public string AffinityOutcome;

    //variable who determines who is the winner
    public string Winner = "";

    //variable used to easily change player moves on Unity inspector
    public int AssignNumberMoves = 0;

    //references to the players in the scene
    public GameObject Player1;
    public Player Player1Player;
    public GameObject Player2;
    public Player Player2Player;
    private AI ai;

    //attacker defender and their tokens
    public GameObject Attacker;
    public Player AttackerController;
    public int[] AttackerTokens = new int[6];
    public GameObject Defender;
    public Player DefenderController;
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
    public MultiDelegate ResetCooldownDelegate;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Map = TransferVariables.statsInstance.Map;
        numberMatches = 0;
        Player1Player = Player1.GetComponent<Player>();

        Player2.tag = "AI";
        Player2Player = Player2.GetComponent<Player>();
        ai = Player2.GetComponent<AI>();

        cameraPosition = Camera.main.transform.position;
        resetGame();
    }

    //resets positions after the current match ends
    private void resetGame()
    {
        MatchWin = false;
        endRound = true;
        numberMatches += 1;
        numRounds = 0;

        Player1Player.Tokens = new int[] { 0, 0, 0, 0, 0, 0 };
        Player2Player.Tokens = new int[] { 0, 0, 0, 0, 0, 0 };
        Player1Player.ExtraMovesReady = true;
        HUD.ExtraMovesButton.image.sprite = HUD.ExtraMovesReady;       

        HUD.ChangeNumberMatch(numberMatches);

        switch (Map)
        {
            case 1:
                Map1.SetActive(true);
                ResetPlayerPosition(new Vector3(4.5f, 2.5f, 0), new Vector3(8.5f, 19.5f, 0));
                cameraPosition.x = 7;
                break;
            case 2:
                Map2.SetActive(true);
                ResetPlayerPosition(new Vector3(15.5f, 3.5f, 0), new Vector3(2.5f, 11.5f, 0));
                cameraPosition.x = 9;
                break;
            case 3:
                break;
            default:
                break;
        }

        ai.MapAI();
        ai.StartAI();

        determineFirstToPlay();

        ResetCooldownDelegate();
    }

    private void ResetPlayerPosition(Vector3 p1pos, Vector3 p2pos)
    {
        Player1Player.ResetPosition(p1pos);
        Player2Player.ResetPosition(p2pos);
    }

    //function to determine who goes first in a match
    private void determineFirstToPlay()
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

    //function to warn player how many tokens he has
    public void TokensLeft(int[] tokens)
    {
        HUD.TokensLeft(tokens);
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
            Player1Player.NumberMovesLeft = AssignNumberMoves;
            Player1Player.HasAttackedThisTurn = false;
            Player1Player.IsInAttackMenu = false;
            Player1Player.EnoughTokensToAttack();


            //adjust camera
            switch (Map)
            {
                case 1:
                    AdjustCamera(5, 17);
                    break;
                case 2:
                    AdjustCamera(6, 8);
                    break;
                case 3:
                    break;
                default:
                    break;
            }

            //re-activate buttons
            HUD.EndTurnButton.interactable = true;
            if (Player1Player.ExtraMovesReady == true)
            {
                HUD.ExtraMovesButton.interactable = true;
            }
            else
            {
                HUD.ExtraMovesButton.interactable = false;
            }
        }
        else if (PlayerTurn == 2)
        {
            Player2Player.NumberMovesLeft = AssignNumberMoves;
            Player2Player.HasAttackedThisTurn = false;
            Player2Player.IsInAttackMenu = false;
            Player2Player.EnoughTokensToAttack();

            if (Player2.CompareTag("AI"))
            {
                ai.MakePath();
            }

            //cameraPosition.x = Player2.transform.position.x;
            //cameraPosition.y = Player2.transform.position.y;

            HUD.EndTurnButton.interactable = false;
            HUD.ExtraMovesButton.interactable = false;
        }

        Camera.main.transform.position = cameraPosition;

        HUD.ChangeTurnRoundHUD(PlayerTurn, numRounds);
    }

    public void AdjustCamera(int min, int max)
    {
        cameraPosition.y = Player1.transform.position.y;
        if (cameraPosition.y < min)
        {
            cameraPosition.y = min;
        }
        if (cameraPosition.y > max)
        {
            cameraPosition.y = max;
        }
    }

    //end round after both players play
    public void EndRound()
    {
        if (endRound == true)
        {
            numRounds += 1;
            endRound = false;
            CooldownDelegate(); //delegate that warns the special tiles that a round has passed
        }
        else
        {
            endRound = true;
        }
    }

    //Attack functions

    //do you want to attack the enemy?
    public void AttackConfirmation(GameObject attackerTemp, GameObject defenderTemp)
    {
        Attacker = attackerTemp;
        Defender = defenderTemp;

        AttackerController = Attacker.GetComponent<Player>();
        DefenderController = Defender.GetComponent<Player>();

        AttackerTokens = AttackerController.Tokens;
        DefenderTokens = DefenderController.Tokens;

        //disable attacker and defender actions while attacker is choosing to attack
        AttackerController.enabled = false;
        DefenderController.enabled = false;

        switch (Attacker.tag)
        {
            case "Player":
                HUD.AttackConfirmationScreen();
                break;
            case "AI":
                if (AttackerController.CanAttack == true)
                {
                    ReadyAttack();
                    HUD.AttackSelectionScreen();
                }
                else
                {
                    CancelAttack();
                }
                break;
            default:
                break;
        }
    }

    //get players ready for battle!
    public void ReadyAttack()
    {
        //re-enable their components
        AttackerController.enabled = true;
        DefenderController.enabled = true;

        //both are now battling
        AttackerController.IsInAttackMenu = true;
        DefenderController.IsInAttackMenu = true;

        //attacker can't attack twice in their turn
        AttackerController.HasAttackedThisTurn = true;
    }

    //function that decides if the players can choose a certain token during battle
    public void CheckTokenAvailability()
    {
        //in case defender doesn't have defense tokens aka Game over
        if (DefenderTokens[1] == 0 && DefenderTokens[3] == 0 && DefenderTokens[5] == 0)
        {
            MatchWin = true;

            AttackerController.matchesWon += 1;

            if (AttackerController.matchesWon == 2)
            {
                Winner = Attacker.name;
            }
            
            HUD.Outcome();
        }

        //check whether Player is attacker or defender then activate the buttons
        switch (AttackerController.PlayerID)
        {
            case 1:

                if (Player1Player.Tokens[0] == 0)
                {
                    HUD.PlayerRButton.interactable = false;
                    HUD.PlayerRButton.image.color = Color.gray;
                }

                if (Player1Player.Tokens[2] == 0)
                {
                    HUD.PlayerPButton.interactable = false;
                    HUD.PlayerPButton.image.color = Color.gray;
                }

                if (Player1Player.Tokens[4] == 0)
                {
                    HUD.PlayerSButton.interactable = false;
                    HUD.PlayerSButton.image.color = Color.gray;
                }

                break;
            case 2:

                if (Player1Player.Tokens[1] == 0)
                {
                    HUD.PlayerRButton.interactable = false;
                    HUD.PlayerRButton.image.color = Color.gray;
                }

                if (Player1Player.Tokens[3] == 0)
                {
                    HUD.PlayerPButton.interactable = false;
                    HUD.PlayerPButton.image.color = Color.gray;
                }

                if (Player1Player.Tokens[5] == 0)
                {
                    HUD.PlayerSButton.interactable = false;
                    HUD.PlayerSButton.image.color = Color.gray;
                }

                break;
            default:
                break;
        }
    }

    //what decides the outcome in case both players have tokens
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

        Player2Player.EnoughTokensToAttack();
        ai.FindAIObjective();

        AttackerController.NumberMovesLeft = 0;
        AttackerController.enabled = true;
        HUD.Outcome();
    }

    public void CancelAttack()
    {
        //re-enable their components in case attacker doesn't want to attack
        AttackerController.enabled = true;
        DefenderController.enabled = true;

        if (Player1Player.ExtraMovesReady == true)
        {
            HUD.ExtraMovesButton.interactable = true;
        }
    }

    public void CheckSacrifice()
    {
        if (Player1Player.Tokens[0] == 0)
        {
            HUD.SacrificeRAToken.interactable = false;
            HUD.SacrificeRAToken.image.color = Color.gray;
        }

        if (Player1Player.Tokens[1] == 0)
        {
            HUD.SacrificeRDToken.interactable = false;
            HUD.SacrificeRDToken.image.color = Color.gray;
        }

        if (Player1Player.Tokens[2] == 0)
        {
            HUD.SacrificePAToken.interactable = false;
            HUD.SacrificePAToken.image.color = Color.gray;
        }

        if (Player1Player.Tokens[3] == 0)
        {
            HUD.SacrificePDToken.interactable = false;
            HUD.SacrificePDToken.image.color = Color.gray;
        }

        if (Player1Player.Tokens[4] == 0)
        {
            HUD.SacrificeSAToken.interactable = false;
            HUD.SacrificeSAToken.image.color = Color.gray;
        }

        if (Player1Player.Tokens[5] == 0)
        {
            HUD.SacrificeSDToken.interactable = false;
            HUD.SacrificeSDToken.image.color = Color.gray;
        }
    }

    public void GetExtraMoves(int tokenChoice)
    {
        //sacrifice tokens
        Player1Player.Tokens[tokenChoice] -= 1;

        //give moves to player
        Player1Player.NumberMovesLeft += Random.Range(1,7);

        HUD.ExtraMovesBox.SetActive(false);

        Player1Player.CancelExtraMoves();

        Player1Player.ExtraMovesEnterCooldown();
    }

    //end the match after one of the players lose (for now it just goes back to the Main Menu)
    public void EndMatch()
    {
        if (Winner != "")
        {
            MainMenu();
        }
        else if (Winner == "" && MatchWin == true)
        {
            resetGame();
        }
    }

    //function that runs when Menu Button in the scene is clicked
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
