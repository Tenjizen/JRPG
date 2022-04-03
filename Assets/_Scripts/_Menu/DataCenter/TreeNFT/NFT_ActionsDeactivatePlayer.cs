using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT_ActionsDeactivatePlayer : MonoBehaviour
{
    [SerializeField] MonstersBase _monstersBase;
    public int nFTNumber;


    public void NFTPlayerChoice()
    {
        switch (nFTNumber)
        {
            case 1:
                NFTPower();
                break;
            case 2:
                NFTComp_1();
                break;
            case 3:
                NFTComp_2();
                break;
            case 4:
                NFTComp_3();
                break;
            case 5:
                NFTComp_4();
                break;
            case 6:
                NFTStat_1();
                break;
            case 7:
                NFTStat_2();
                break;
            case 8:
                NFTStat_3();
                break;
            case 9:
                NFTStat_4();
                break;
            case 10:
                NFTStat_5();
                break;
            case 11:
                NFTStat_6();
                break;
            case 12:
                NFTStat_7();
                break;
            case 13:
                NFTStat_8();
                break;
            case 14:
                NFTStat_9();
                break;
            case 15:
                NFTStat_10();
                break;
            case 16:
                NFTStat_11();
                break;
            case 17:
                NFTStat_12();
                break;
            case 18:
                NFTStat_13();
                break;
            case 19:
                NFTStat_14();
                break;
        }
    }

    private void NFTPower()
    {
        Debug.Log("Pouvoir désactivé");
    }
    private void NFTComp_1()
    {
        Debug.Log("Comp 1 désactivé");
    }
    private void NFTComp_2()
    {
        Debug.Log("Comp 2 désactivé");
    }
    private void NFTComp_3()
    {
        Debug.Log("Comp 3 désactivé");
    }
    private void NFTComp_4()
    {
        Debug.Log("Comp 4 désactivé");
    }
    private void NFTStat_1()
    {
        Debug.Log("Stat 1 désactivé");
    }
    private void NFTStat_2()
    {
        Debug.Log("Stat 2 désactivé");
    }
    private void NFTStat_3()
    {
        Debug.Log("Stat 3 désactivé");
    }
    private void NFTStat_4()
    {
        Debug.Log("Stat 4 désactivé");
    }
    private void NFTStat_5()
    {
        Debug.Log("Stat 5 désactivé");
    }
    private void NFTStat_6()
    {
        Debug.Log("Stat 6 désactivé");
    }
    private void NFTStat_7()
    {
        Debug.Log("Stat 7 désactivé");
    }
    private void NFTStat_8()
    {
        Debug.Log("Stat 8 désactivé");
    }
    private void NFTStat_9()
    {
        Debug.Log("Stat 9 désactivé");
    }
    private void NFTStat_10()
    {
        Debug.Log("Stat 10 désactivé");
    }
    private void NFTStat_11()
    {
        Debug.Log("Stat 11 désactivé");
    }
    private void NFTStat_12()
    {
        Debug.Log("Stat 12 désactivé");
    }
    private void NFTStat_13()
    {
        Debug.Log("Stat 13 désactivé");
    }
    private void NFTStat_14()
    {
        Debug.Log("Stat 14 désactivé");
    }
}
