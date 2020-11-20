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

    //references to the text that shows who's tokens belongs to
    public Text AttackerTokensUI;
    public Text DefenderTokensUI;

    //references to the Outcome's text
    public Text OutcomeUIText;

    //references to the AttackSelectionBox buttons
    public Button AttackerRButton;
    public Button AttackerPButton;
    public Button AttackerSButton;
    public Button DefenderRButton;
    public Button DefenderPButton;
    public Button DefenderSButton;

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

        //temporary while we hotsitting
        AttackerTokensUI.text = GameplayManager.Attacker.name;
        DefenderTokensUI.text = GameplayManager.Defender.name;

        //ready attacker buttons
        AttackerRButton.interactable = true;
        AttackerRButton.image.color = Color.red;
        AttackerPButton.interactable = true;
        AttackerPButton.image.color = Color.green;
        AttackerSButton.interactable = true;
        AttackerSButton.image.color = Color.blue;

        //ready defender buttons
        DefenderRButton.interactable = true;
        DefenderRButton.image.color = Color.red;
        DefenderPButton.interactable = true;
        DefenderPButton.image.color = Color.green;
        DefenderSButton.interactable = true;
        DefenderSButton.image.color = Color.blue;

        GameplayManager.CheckTokenAvailability();
    }

    //when Attacker chooses a token
    public void AttackersChoice(string choice)
    {
        AttackerChoice = choice;

        //Disable buttons
        AttackerRButton.interactable = false;
        AttackerRButton.image.color = Color.gray;
        AttackerPButton.interactable = false;
        AttackerPButton.image.color = Color.gray;
        AttackerSButton.interactable = false;
        AttackerSButton.image.color = Color.gray;
    }

    //when Defender chooses a token
    public void DefendersChoice(string choice)
    {
        DefenderChoice = choice;

        //Disable buttons
        DefenderRButton.interactable = false;
        DefenderRButton.image.color = Color.gray;
        DefenderPButton.interactable = false;
        DefenderPButton.image.color = Color.gray;
        DefenderSButton.interactable = false;
        DefenderSButton.image.color = Color.gray;
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
