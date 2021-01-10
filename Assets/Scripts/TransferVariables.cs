using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
