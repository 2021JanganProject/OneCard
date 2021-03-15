using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private CardManager cardManager;
    [SerializeField] private bool orderDirection = true;
    private int MaxPlayerCount;
    private int playerCount = 0;
    private int currentTurnPlayer;
    private int[] orderPlayers = new int[4];
    [SerializeField] Text orderTest;

    public void TurnEnd()
    {
        if(orderDirection)
        {
            if(playerCount < 3)
            {
                playerCount += 1;
            }
            else
            {
                playerCount -= 3;
            }
        }
        else
        {
            if (playerCount > 0)
            {
                playerCount -= 1;
            }
            else
            {
                playerCount += 3;
            }
        }
        orderTest.text = orderPlayers[playerCount].ToString();
    }

    public void ReverseTurn()
    {
        orderDirection = !orderDirection;
    }

    private void Start()
    {
        orderPlayers[0] = 1;
        orderPlayers[1] = 2;
        orderPlayers[2] = 3;
        orderPlayers[3] = 4;
    }

    private void NextCurrentTurnPlayer()
    {

    }



    private void OrderPlayer()
    {

    }



}
