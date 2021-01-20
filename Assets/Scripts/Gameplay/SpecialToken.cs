using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialToken : MonoBehaviour
{
    public bool OffCooldown = true;
    private int roundsCooldownLeft = 0;
    private string nameTile;
    string specialTile;
    public Animator AnimatorManager;
    public GameObject Player2;

    //references to tile cooldown sprites
    public Sprite TileCooldown4;
    public Sprite TileCooldown3;
    public Sprite TileCooldown2;
    public Sprite TileCooldown1;

    private void Awake()
    {
        //tag that determines type of tile
        specialTile = gameObject.tag;

        //Delegate ChangeCooldown to CooldownDelegate in the Gameplay Manager
        GameplayManager.instance.CooldownDelegate += ChangeCooldown;

        //Delegate ResetCooldown to ResetCooldown Delegate in the Gameplay Manager
        GameplayManager.instance.ResetCooldownDelegate += ResetCooldown;

        Animation();
    }

    //function that decides what animation to play based on tile tag
    private void Animation()
    {
        switch (specialTile)
        {
            case "RedSpecialTile":
                AnimatorManager.SetInteger("Color", 1);
                break;
            case "GreenSpecialTile":
                AnimatorManager.SetInteger("Color", 2);
                break;
            case "BlueSpecialTile":
                AnimatorManager.SetInteger("Color", 3);
                break;
            default:
                break;
        }
    }

    //when player enters the special tile
    //only gets tokens if number of attack or defense tokens below 2
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("AI")) && OffCooldown == true)
        {
            GameObject PlayerCollider = collision.gameObject;
            Player PlayerColliderController = PlayerCollider.GetComponent<Player>();

            int NumberAttackTokens = PlayerColliderController.Tokens[0] + PlayerColliderController.Tokens[2] + PlayerColliderController.Tokens[4];
            int NumberDefenseTokens = PlayerColliderController.Tokens[1] + PlayerColliderController.Tokens[3] + PlayerColliderController.Tokens[5];

            bool SpecialTileActivated = false;

            if (NumberAttackTokens < 2)
            {
                switch (specialTile)
                {
                    case "RedSpecialTile":
                        PlayerColliderController.Tokens[0] += 1;
                        break;
                    case "GreenSpecialTile":
                        PlayerColliderController.Tokens[2] += 1;
                        break;
                    case "BlueSpecialTile":
                        PlayerColliderController.Tokens[4] += 1;
                        break;
                    default:
                        break;
                }

                SpecialTileActivated = true;
            }

            if (NumberDefenseTokens < 2)
            {
                switch (specialTile)
                {
                    case "RedSpecialTile":
                        PlayerColliderController.Tokens[1] += 1;
                        break;
                    case "GreenSpecialTile":
                        PlayerColliderController.Tokens[3] += 1;
                        break;
                    case "BlueSpecialTile":
                        PlayerColliderController.Tokens[5] += 1;
                        break;
                    default:
                        break;
                }

                SpecialTileActivated = true;
            }

            //this only happens if the player/ai gets a token aka activates the tile
            if (SpecialTileActivated == true)
            {
                EnterCooldown();

                PlayerColliderController.EnoughTokensToAttack();
                Player2.GetComponent<AI>().FindAIObjective();
            }
        }
    }

    private void EnterCooldown()
    {
        OffCooldown = false;
        AnimatorManager.enabled = false;
        roundsCooldownLeft = 5;
        ChangeCooldown();
    }

    public void ChangeCooldown()
    {
        roundsCooldownLeft -= 1;

        switch (roundsCooldownLeft)
        {
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = TileCooldown4;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = TileCooldown3;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = TileCooldown2;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = TileCooldown1;
                break;
            case 0:
                AnimatorManager.enabled = true;
                OffCooldown = true;
                break;
            default:
                break;
        }
    }

    public void ResetCooldown()
    {
        roundsCooldownLeft = 0;
        OffCooldown = true;
        AnimatorManager.enabled = true;
    }
}
