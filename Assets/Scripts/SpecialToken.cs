using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialToken : MonoBehaviour
{
    private bool OffCooldown = true;
    private int RoundsCooldownLeft = 0;
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

    //references to special tile sprites (temporary until we implement animations)
    public Sprite RedTile;
    public Sprite GreenTile;
    public Sprite BlueTile;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && OffCooldown == true)
        {
            GameObject PlayerCollider = collision.gameObject;

            switch (specialTile)
            {
                case "RedSpecialTile":
                    PlayerCollider.GetComponent<Player>().Tokens[0] += 1;
                    PlayerCollider.GetComponent<Player>().Tokens[1] += 1;
                    break;
                case "GreenSpecialTile":
                    PlayerCollider.GetComponent<Player>().Tokens[2] += 1;
                    PlayerCollider.GetComponent<Player>().Tokens[3] += 1;
                    break;
                case "BlueSpecialTile":
                    PlayerCollider.GetComponent<Player>().Tokens[4] += 1;
                    PlayerCollider.GetComponent<Player>().Tokens[5] += 1;
                    break;
                default:
                    break;
            }

            EnterCooldown();
        }
    }

    private void EnterCooldown()
    {
        OffCooldown = false;
        AnimatorManager.enabled = false;
        RoundsCooldownLeft = 5;
        ChangeCooldown();
    }

    public void ChangeCooldown()
    {
        RoundsCooldownLeft -= 1;

        switch (specialTile)
        {
            case "RedSpecialTile":
                switch (RoundsCooldownLeft)
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
                switch (RoundsCooldownLeft)
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
                switch (RoundsCooldownLeft)
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
