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
    public Button EndTurnButton;
    public Button MainMenuButton;
    public GameObject AttackConfirmationBox;
    public GameObject AttackSelectionBox;
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

    public void ChangeTurnRoundHUD(int playerturn, int numRounds)
    {
        WhosTurn.text = $"Player {playerturn}'s turn";
        NumRounds.text = "Round " + numRounds;
    }

    public void AttackConfirmationScreen()
    {
        AttackConfirmationBox.SetActive(true);

        if (GameplayManager.Attacker.GetComponent<Player>().CanAttack == true)
        {
            Question.GetComponent<Text>().text = "Do you want to attack the enemy?";
            Yes.gameObject.SetActive(true);
            No.gameObject.SetActive(true);
            Ok.gameObject.SetActive(false);
        }
        else
        {
            Question.GetComponent<Text>().text = "Can't attack the enemy.";
            Yes.gameObject.SetActive(false);
            No.gameObject.SetActive(false);
            Ok.gameObject.SetActive(true);

        }

        //disable other buttons' functions while the player decides to attack
        EndTurnButton.GetComponent<Button>().interactable = false;
        MainMenuButton.GetComponent<Button>().interactable = false;
    }

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

    public void Player1Choice(string choice)
    {
        switch (GameplayManager.Attacker.GetComponent<Player>().PlayerID)
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

    public void AIChoice(string side)
    {
        int choice = -1;

        while (choice == -1)
        {
            choice = Random.Range(0, 6);

            switch (side)
            {
                case "Attacker":
                    if (choice % 2 == 1)
                    {
                        choice--;
                    }

                    if (GameplayManager.AttackerTokens[choice] == 0)
                    {
                        choice = -1;
                    }

                    break;
                case "Defender":
                    if (choice % 2 == 0)
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
}
