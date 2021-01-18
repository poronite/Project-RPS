using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I know this is unacceptable but for now it works

public class TransferVariables : MonoBehaviour
{
    public static TransferVariables statsInstance = null;

    public int Map;

    private void Awake()
    {
        if (statsInstance == null)
        {
            statsInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
