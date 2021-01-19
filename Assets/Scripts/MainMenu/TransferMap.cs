using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    public static TransferMap TransferMapInst = null;

    //this will also be used to decide the BGM 
    //if 0 it means it is in the Main Menu
    public int Map = 0;

    private void Awake()
    {
        if (TransferMapInst == null)
        {
            TransferMapInst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
