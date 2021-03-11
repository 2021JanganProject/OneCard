using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance =null;
    private CardManager cardManager;
    private bool orderDirection;
    private int MaxPlayerCount;
    private int playerCount;
    private int PlayerCount;
    private int currentTurnPlayer;
    private int CurrentTurnPlayer;
    private int[] orderPlayers;

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void NextCurrentTurnPlayer()
    {

    }

    private void ReverseTurn()
    {

    }

    private void OrderPlayer()
    {

    }

    private void TurnEnd()
    {

    }

}
