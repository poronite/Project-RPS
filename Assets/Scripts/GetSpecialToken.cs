using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpecialToken : MonoBehaviour
{
    //variable that shows if tile can give token
    public bool OffCooldown = true;
    private int RoundsCooldownLeft;
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
        RoundsCooldownLeft = 3;
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
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    case 2:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    case 1:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    default:
                        break;
                }
                break;
            case "Green":
                switch (RoundsCooldownLeft)
                {
                    case 3:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    case 2:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    case 1:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    default:
                        break;
                }
                break;
            case "Blue":
                switch (RoundsCooldownLeft)
                {
                    case 3:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    case 2:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
                        break;
                    case 1:
                        //gameObject.GetComponent<SpriteRenderer>().sprite = ;
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
