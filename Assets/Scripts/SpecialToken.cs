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

    //references to tile cooldown sprites
    public Sprite RedCooldown4;
    public Sprite RedCooldown3;
    public Sprite RedCooldown2;
    public Sprite RedCooldown1;
    public Sprite GreenCooldown4;
    public Sprite GreenCooldown3;
    public Sprite GreenCooldown2;
    public Sprite GreenCooldown1;
    public Sprite BlueCooldown4;
    public Sprite BlueCooldown3;
    public Sprite BlueCooldown2;
    public Sprite BlueCooldown1;

    private void Start()
    {
        //tag that determines type of tile
        specialTile = gameObject.tag;

        //Delegate ChangeCooldown to CooldownDelegate in the Gameplay Manager
        GameplayManager.instance.CooldownDelegate += ChangeCooldown;

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
                PlayerColliderController.EnoughTokensToAttack();

                if (PlayerCollider.CompareTag("AI"))
                {
                    PlayerCollider.GetComponent<AI>().FindAIObjective();
                }

                EnterCooldown();
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

        switch (specialTile)
        {
            case "RedSpecialTile":
                switch (roundsCooldownLeft)
                {
                    case 4:
                        gameObject.GetComponent<SpriteRenderer>().sprite = RedCooldown4;
                        break;
                    case 3:
                        gameObject.GetComponent<SpriteRenderer>().sprite = RedCooldown3;
                        break;
                    case 2:
                        gameObject.GetComponent<SpriteRenderer>().sprite = RedCooldown2;
                        break;
                    case 1:
                        gameObject.GetComponent<SpriteRenderer>().sprite = RedCooldown1;
                        break;
                    case 0:
                        AnimatorManager.enabled = true;
                        OffCooldown = true;   
                        break;
                    default:
                        break;
                }
                break;
            case "GreenSpecialTile":
                switch (roundsCooldownLeft)
                {
                    case 4:
                        gameObject.GetComponent<SpriteRenderer>().sprite = GreenCooldown4;
                        break;
                    case 3:
                        gameObject.GetComponent<SpriteRenderer>().sprite = GreenCooldown3;
                        break;
                    case 2:
                        gameObject.GetComponent<SpriteRenderer>().sprite = GreenCooldown2;
                        break;
                    case 1:
                        gameObject.GetComponent<SpriteRenderer>().sprite = GreenCooldown1;
                        break;
                    case 0:
                        AnimatorManager.enabled = true;
                        OffCooldown = true;
                        break;
                    default:
                        break;
                }
                break;
            case "BlueSpecialTile":
                switch (roundsCooldownLeft)
                {
                    case 4:
                        gameObject.GetComponent<SpriteRenderer>().sprite = BlueCooldown4;
                        break;
                    case 3:
                        gameObject.GetComponent<SpriteRenderer>().sprite = BlueCooldown3;
                        break;
                    case 2:
                        gameObject.GetComponent<SpriteRenderer>().sprite = BlueCooldown2;
                        break;
                    case 1:
                        gameObject.GetComponent<SpriteRenderer>().sprite = BlueCooldown1;
                        break;
                    case 0:
                        AnimatorManager.enabled = true;
                        OffCooldown = true;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
