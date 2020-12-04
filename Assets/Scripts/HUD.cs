using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //reference to GameplayManager
    public GameplayManager GameplayManager;

    //references to HUD elements in the scene
    public Text WhosTurn;
    public Text CurrentPlayerMovesLeft;
    public Text NumRounds;
    public Text NumRTokens;
    public Text NumPTokens;
    public Text NumSTokens;
    public Button EndTurnButton;
    public Button ExtraMovesButton;
    public Text ExtraMovesText;
    public Button MainMenuButton;
    public GameObject AttackConfirmationBox;
    public GameObject AttackSelectionBox;
    public GameObject ExtraMovesBox;
    public GameObject OutcomeUI;
     

    //references to the AttackConfirmationBox's UI elements
    public Text Question;
    public Button Yes;
    public Button No;
    public Button Ok;

    //references to the Outcome's text
    public Text OutcomeUIText;

    //references to the AttackSelectionBox buttons
    public Button PlayerRButton;
    public Button PlayerPButton;
    public Button PlayerSButton;

    //references to the ExtraMovesBox buttons
    public Button SacrificeRAToken;
    public Button SacrificePAToken;
    public Button SacrificeSAToken;
    public Button SacrificeRDToken;
    public Button SacrificePDToken;
    public Button SacrificeSDToken;

    //strings that hold the player decisions
    public string AttackerChoice = "";
    public string DefenderChoice = "";

    private void Update()
    {
        //this only runs when both players choose their attack/defense
        if (AttackerChoice != "" && DefenderChoice != "")
        {
            GameplayManager.AttackSystem(AttackerChoice, DefenderChoice);

            AttackSelectionBox.SetActive(false);

            AttackerChoice = "";
            DefenderChoice = "";
        }
    }

    //display moves left
    public void MovesLeftHUD(int movesleft)
    {
        if (movesleft > 0)
        {
            CurrentPlayerMovesLeft.text = $"{movesleft} moves left";
            EndTurnButton.image.color = new Color(255, 255, 255);
        }
        else if (movesleft == 0)
        {
            CurrentPlayerMovesLeft.text = "No moves left";
            EndTurnButton.image.color = new Color(186, 253, 0);
        }
    }

    //display number of tokens
    public void TokensLeft(int[] tokens)
    {
        NumRTokens.text = $"Randomness Tokens: {tokens[0]}/{tokens[1]}";
        NumPTokens.text = $"Patience Tokens: {tokens[2]}/{tokens[3]}";
        NumSTokens.text = $"Strategy Tokens: {tokens[4]}/{tokens[5]}";
    }

    //display round and player turn
    public void ChangeTurnRoundHUD(int playerturn, int numRounds)
    {
        WhosTurn.text = $"Player {playerturn}'s turn";
        NumRounds.text = "Round " + numRounds;
    }

    //attack confirmation screen box
    public void AttackConfirmationScreen()
    {
        AttackConfirmationBox.SetActive(true);

        if (GameplayManager.AttackerController.CanAttack == true)
        {
            Question.text = "Do you want to attack the enemy?";
            Yes.gameObject.SetActive(true);
            No.gameObject.SetActive(true);
            Ok.gameObject.SetActive(false);
        }
        else
        {
            Question.text = "Can't attack the enemy.";
            Yes.gameObject.SetActive(false);
            No.gameObject.SetActive(false);
            Ok.gameObject.SetActive(true);

        }

        //disable other buttons' functions while the player decides to attack
        EndTurnButton.interactable = false;
        ExtraMovesButton.interactable = false;
        MainMenuButton.interactable = false;
    }

    //attack selection screen box
    public void AttackSelectionScreen()
    {
        AttackSelectionBox.SetActive(true);

        PlayerRButton.interactable = true;
        PlayerRButton.image.color = Color.red;

        PlayerPButton.interactable = true;
        PlayerPButton.image.color = Color.green;

        PlayerSButton.interactable = true;
        PlayerSButton.image.color = Color.blue;

        GameplayManager.CheckTokenAvailability();
    }

    public void Player1Choice(string choice) //function that runs when player chooses a token when attacking/defending
    {
        switch (GameplayManager.AttackerController.PlayerID)
        {
            case 1:
                AttackersChoice(choice);
                AIChoice("Defender");
                break;
            case 2:
                DefendersChoice(choice);
                AIChoice("Attacker");
                break;
            default:
                break;
        }
    }

    public void AIChoice(string side) //function that runs when player chooses his token
    {
        int choice = -1;

        while (choice == -1)
        {
            choice = Random.Range(0, 6);

            switch (side)
            {
                case "Attacker":
                    if (choice % 2 == 1) //attack token are in even indexes of the token's array so this prevents the AI from picking a defense token
                    {
                        choice--;
                    }

                    if (GameplayManager.AttackerTokens[choice] == 0)
                    {
                        choice = -1;
                    }

                    break;
                case "Defender":
                    if (choice % 2 == 0) //same logic as above
                    {
                        choice++;
                    }

                    if (GameplayManager.DefenderTokens[choice] == 0)
                    {
                        choice = -1;
                    }

                    break;
                default:
                    break;
            }
        }
        
        switch (choice)
        {
            case 0:
                AttackersChoice("Randomness");
                break;
            case 1:
                DefendersChoice("Randomness");
                break;
            case 2:
                AttackersChoice("Patience");
                break;
            case 3:
                DefendersChoice("Patience");
                break;
            case 4:
                AttackersChoice("Strategy");
                break;
            case 5:
                DefendersChoice("Strategy");
                break;
            default:
                break;
        }
    }

    //when Attacker chooses a token
    public void AttackersChoice(string choice)
    {
        AttackerChoice = choice;
    }

    //when Defender chooses a token
    public void DefendersChoice(string choice)
    {
        DefenderChoice = choice;
    }

    //box with text that shows outcome of the battle
    public void Outcome()
    {
        AttackSelectionBox.SetActive(false);
        OutcomeUI.SetActive(true);

        if (GameplayManager.Winner != "")
        {
            OutcomeUIText.text = $"{GameplayManager.Winner} wins!";
        }
        else
        {
            switch (GameplayManager.AffinityOutcome)
            {
                case "Advantage":
                    OutcomeUIText.text = $"Attack successful! {GameplayManager.Attacker.name} consumes {AttackerChoice} Attack Token. {GameplayManager.Defender.name} loses {DefenderChoice} Defense token. {GameplayManager.Attacker.name} receives {DefenderChoice} Attack Token.";
                    break;
                case "Disadvantage":
                    OutcomeUIText.text = $"Attack unsucessful! {GameplayManager.Attacker.name} loses {AttackerChoice} Attack Token.";
                    break;
                case "Neutral":
                    OutcomeUIText.text = $"It's a draw! {GameplayManager.Attacker.name} loses {AttackerChoice} Attack Token. {GameplayManager.Defender.name} loses {DefenderChoice} Defense Token.";
                    break;
                default:
                    break;
            }
        }
    }

    public void ExtraMovesScreen()
    {
        GameplayManager.Player1Player.IsInMenu = true;

        SacrificeRAToken.image.color = Color.red;
        SacrificeRAToken.interactable = true;
        SacrificeRDToken.image.color = Color.red;
        SacrificeRDToken.interactable = true;

        SacrificePAToken.image.color = Color.green;
        SacrificePAToken.interactable = true;
        SacrificePDToken.image.color = Color.green;
        SacrificePDToken.interactable = true;

        SacrificeSAToken.image.color = Color.blue;
        SacrificeSAToken.interactable = true;
        SacrificeSDToken.image.color = Color.blue;
        SacrificeSDToken.interactable = true;

        ExtraMovesBox.SetActive(true);

        GameplayManager.CheckSacrifice();
    }
}
