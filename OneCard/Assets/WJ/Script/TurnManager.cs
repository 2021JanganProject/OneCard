using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private int MaxPlayerCount = 4;

    //private Player player;

    private int playerCount;
    private int currentTurnPlayer;
    //private Player[] orderPlayers;
    //private List<int> orderList = new List<int>();
    private bool orderDirection = true; // true면 시계 방향

    
    public int PlayerCount
    {
        get
        {
            return playerCount;
        }
        set
        {
            if (PlayerCount <= MaxPlayerCount)
            {
                playerCount = value;
            }
        }
    }
    public int CurrentTurnPlayer
    {
        get
        {
            return currentTurnPlayer;
        }
        set
        {
            if (currentTurnPlayer > 0 && currentTurnPlayer <= MaxPlayerCount)
                currentTurnPlayer = value;
        }
    }
    void TurnEnd()
    {
        //
    }

    void ReverseTurn()
    {
        orderDirection = !orderDirection;
    }

    void OrderPlayer()
    {
        
        //플레이어 순서 정하기
    }
}
