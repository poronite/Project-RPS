using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpecialToken : MonoBehaviour
{
    //variable that shows if tile can give token
    public bool OffCooldown = true;
    private int RoundsCooldownLeft = 0;
    private string nameTile;

    //references to tile cooldown sprites
    public Sprite RedCooldown3;
    public Sprite RedCooldown2;
    public Sprite RedCooldown1;
    public Sprite GreenCooldown3;
    public Sprite GreenCooldown2;
    public Sprite GreenCooldown1;
    public Sprite BlueCooldown3;
    public Sprite BlueCooldown2;
    public Sprite BlueCooldown1;

    //references to special tile sprites (temporary until we implement animations)
    public Sprite RedTile;
    public Sprite GreenTile;
    public Sprite BlueTile;

    private void Start()
    {
        //Delegate ChangeCooldown to CooldownDelegate in the Gameplay Manager
        GameplayManager.instance.CooldownDelegate += ChangeCooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && OffCooldown == true)
        {
            string specialTile = gameObject.tag;
            GameObject PlayerCollider = collision.gameObject;

            switch (specialTile)
            {
                case "RedSpecialTile":
                    PlayerCollider.GetComponent<Player>().Tokens[0] += 1;
                    PlayerCollider.GetComponent<Player>().Tokens[1] += 1;
                    nameTile = "Red";
                    break;
                case "GreenSpecialTile":
                    PlayerCollider.GetComponent<Player>().Tokens[2] += 1;
                    PlayerCollider.GetComponent<Player>().Tokens[3] += 1;
                    nameTile = "Green";
                    break;
                case "BlueSpecialTile":
                    PlayerCollider.GetComponent<Player>().Tokens[4] += 1;
                    PlayerCollider.GetComponent<Player>().Tokens[5] += 1;
                    nameTile = "Blue";
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
        RoundsCooldownLeft = 4;
        ChangeCooldown();
    }

    public void ChangeCooldown()
    {
        RoundsCooldownLeft -= 1;

        switch (nameTile)
        {
            case "Red":
                switch (RoundsCooldownLeft)
                {
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
                        gameObject.GetComponent<SpriteRenderer>().sprite = RedTile;
                        OffCooldown = true;
                        break;
                    default:
                        break;
                }
                break;
            case "Green":
                switch (RoundsCooldownLeft)
                {
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
                        gameObject.GetComponent<SpriteRenderer>().sprite = GreenTile;
                        OffCooldown = true;
                        break;
                    default:
                        break;
                }
                break;
            case "Blue":
                switch (RoundsCooldownLeft)
                {
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
                        gameObject.GetComponent<SpriteRenderer>().sprite = BlueTile;
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
