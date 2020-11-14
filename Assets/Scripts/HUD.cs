using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //references to HUD elements in the scene
    public Text WhosTurn;
    public Text CurrentPlayerMovesLeft;
    public Text NumRounds;
    public Button EndTurnButton;

    public void MovesLeftHUD(int movesleft)
    {
        if (movesleft > 0)
        {
            CurrentPlayerMovesLeft.text = $"{movesleft} moves left";
            EndTurnButton.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        else if (movesleft == 0)
        {
            CurrentPlayerMovesLeft.text = "No moves left";
            EndTurnButton.GetComponent<Image>().color = new Color(186, 253, 0);
        }
    }

    public void ChangeTurnRoundHUD(int playerturn, int numRounds)
    {
        WhosTurn.text = $"Player {playerturn}'s turn";
        NumRounds.text = "Round " + numRounds;
    }
}
