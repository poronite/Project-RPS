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
    public Text NumMatches;
    public Text NumRounds;

    public Text NumRAtkTokens;
    public Text NumRDefTokens;
    public Text NumPAtkTokens;
    public Text NumPDefTokens;
    public Text NumSAtkTokens;
    public Text NumSDefTokens;

    public Button EndTurnButton;
    public Button ExtraMovesButton;
    public Button MainMenuButton;
    public GameObject AttackConfirmationBox;
    public GameObject AttackSelectionBox;
    public GameObject ExtraMovesBox;
    public GameObject OutcomeUI;

    //references to Extra Moves Cooldown images
    public Sprite ExtraMoves4;
    public Sprite ExtraMoves3;
    public Sprite ExtraMoves2;
    public Sprite ExtraMoves1;
    public Sprite ExtraMovesReady;

    //references to the AttackConfirmationBox's UI elements
    public Button AttackButton;
    public Button RefuseAttackButton;

    //screen gets darker when player is choosing the token
    public GameObject Filter;

    public Animator AttackerAnimator;
    public Animator DefenderAnimator;
    public Image AttackerImage;
    public Image DefenderImage;

    //references to the Outcome's elements
    public Text OutcomeUIText;
    public GameObject AttackerAvatar;
    public GameObject DefenderAvatar;
    public GameObject AttackerLostToken;
    public GameObject AttackerObtainedToken;
    public GameObject DefenderLostToken;

    //references to the AttackSelectionBox buttons
    public Button PlayerRButton;
    public Button PlayerPButton;
    public Button PlayerSButton;

    //images
    public Sprite Player1Image;
    public Sprite Player2Image;

    //animations
    public RuntimeAnimatorController RedAttackToken;
    public RuntimeAnimatorController RedDefenseToken;
    public RuntimeAnimatorController GreenAttackToken;
    public RuntimeAnimatorController GreenDefenseToken;
    public RuntimeAnimatorController BlueAttackToken;
    public RuntimeAnimatorController BlueDefenseToken;
    public RuntimeAnimatorController Player1Animation;
    public RuntimeAnimatorController Player2Animation;

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
            EndTurnButton.image.color = new Color32(255, 255, 255, 255);
        }
        else if (movesleft == 0)
        {
            CurrentPlayerMovesLeft.text = "No moves left";
            EndTurnButton.image.color = new Color32(255, 242, 136, 255);
        }
    }

    //display number of tokens
    public void TokensLeft(int[] tokens)
    {
        NumRAtkTokens.text = tokens[0] == 0 ? "" : $"{tokens[0]}";
        NumRDefTokens.text = tokens[1] == 0 ? "" : $"{tokens[1]}";
        NumPAtkTokens.text = tokens[2] == 0 ? "" : $"{tokens[2]}";
        NumPDefTokens.text = tokens[3] == 0 ? "" : $"{tokens[3]}";
        NumSAtkTokens.text = tokens[4] == 0 ? "" : $"{tokens[4]}";
        NumSDefTokens.text = tokens[5] == 0 ? "" : $"{tokens[5]}";
    }

    //display round and player turn
    public void ChangeTurnRoundHUD(int playerturn, int numRounds)
    {
        if (playerturn == 1)
        {
            WhosTurn.color = new Color32(82, 98, 255, 255);
        }
        else if (playerturn == 2)
        {
            WhosTurn.color = new Color32(255, 82, 98, 255);
        }

        WhosTurn.text = $"Player {playerturn}'s turn";
        NumRounds.text = "Round " + numRounds;
    }

    //attack confirmation screen box
    public void AttackConfirmationScreen()
    {
        AttackConfirmationBox.SetActive(true);

        if (GameplayManager.AttackerController.CanAttack == true)
        {
            AttackButton.gameObject.SetActive(true);
            RefuseAttackButton.gameObject.SetActive(true);

            //disable other buttons' functions while the player decides to attack
            EndTurnButton.interactable = false;
            ExtraMovesButton.interactable = false;
            MainMenuButton.interactable = false;
        }
        else
        {
            AttackConfirmationBox.SetActive(false);
            GameplayManager.CancelAttack();
        }

        
    }

    //attack selection screen box
    public void AttackSelectionScreen()
    {
        Filter.SetActive(true);
        AttackSelectionBox.SetActive(true);

        PlayerRButton.interactable = true;
        PlayerRButton.animator.enabled = true;
        PlayerPButton.interactable = true;
        PlayerPButton.animator.enabled = true;
        PlayerSButton.interactable = true;
        PlayerSButton.animator.enabled = true;

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

    //images that represent the outcome of the battle
    //for whatever reason the animation of the player avatars isn't working
    public void Outcome()
    {
        if (GameplayManager.AttackerController.PlayerID == 1)
        {
            AttackerAnimator.runtimeAnimatorController = Player1Animation;
            AttackerImage.sprite = Player1Image;
            DefenderAnimator.runtimeAnimatorController = Player2Animation;
            DefenderImage.sprite = Player2Image;
        }
        else
        {
            AttackerAnimator.runtimeAnimatorController = Player2Animation;
            AttackerImage.sprite = Player2Image;
            DefenderAnimator.runtimeAnimatorController = Player1Animation;
            DefenderImage.sprite = Player1Image;
        }

        AttackSelectionBox.SetActive(false);
        OutcomeUI.SetActive(true);

        OutcomeUIText.text = "Battle Outcome";
        AttackerLostToken.SetActive(false);
        AttackerObtainedToken.SetActive(false);
        DefenderLostToken.SetActive(false);

        AttackerAnimator.enabled = false;
        DefenderAnimator.enabled = false;

        if (GameplayManager.Winner == "" && GameplayManager.MatchWin == true) //Winning a match
        {
            OutcomeUIText.text = $"{GameplayManager.Attacker.name} wins this match.";

            AttackerAnimator.enabled = true;
        }
        else if (GameplayManager.Winner != "")
        {
            OutcomeUIText.text = $"{GameplayManager.Winner} wins the game!";

            AttackerAnimator.enabled = true;
        }
        else if(GameplayManager.Winner == "" && GameplayManager.MatchWin == false)
        {
            AttackerLostToken.SetActive(true);

            switch (GameplayManager.AffinityOutcome)
            {
                case "Advantage":
                    AttackerObtainedToken.SetActive(true);
                    DefenderLostToken.SetActive(true);
                    AttackerAnimator.enabled = true;
                    break;
                case "Disadvantage":
                    AttackerLostToken.SetActive(true);
                    DefenderAnimator.enabled = true;
                    break;
                case "Neutral":
                    AttackerLostToken.SetActive(true);
                    DefenderLostToken.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    public void ExtraMovesScreen()
    {
        GameplayManager.Player1Player.IsInAttackMenu = true;
        
        SacrificeRAToken.interactable = true;
        SacrificeRDToken.interactable = true;
        
        SacrificePAToken.interactable = true;
        SacrificePDToken.interactable = true;
        
        SacrificeSAToken.interactable = true;
        SacrificeSDToken.interactable = true;

        ExtraMovesBox.SetActive(true);

        GameplayManager.CheckSacrifice();
    }

    public void ChangeNumberMatch(int numMatches)
    {
        NumMatches.text = "Match " + numMatches;

        NumRounds.text = "Round " + 0;
    }
}
