using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //서버에서 플레이어 데이터를 받아와서 배열에 넣어줌
    [SerializeField]
    private static int MaxPlayerCount = 4;

    private Player currentPlayer; // 현재 턴인 플레이어 받을 변수 -> 리스트0번쨰 플레이어가 계속 들어갈거임

    private int playerCount = 0;
    private int currentTurnPlayer = 0;//현재 턴인 플레이어 인덱스 -> List첫번쨰 플레이어가 턴이니깐 계속 0이면 될듯

    [SerializeField]
    private Player[] orderPlayers; // 들어온 플레이어 보관함
    [SerializeField]
    private List<Player> orderList = new List<Player>();  // 실질적인 턴을 결정함
    private bool orderDirection = true; // true면 시계 방향
    

    private UIClock clockUI;

    private PlayerState myTurn = PlayerState.myTurn;
    private PlayerState NextTurn = PlayerState.NextTurn;
    private PlayerState Wait = PlayerState.Wait;

    public Player[] getorderPlaeyrs()
    {
        return orderPlayers;
    }
    public List<Player> getorderList()
    {
        return orderList;
    }
    private void Awake()
    {
        //Player배열에 들어온 순서대로 플레이어들 받아줌
        //Player 배열에서 Player List로 쏴줌 -> 쏴줄 떄 무작위로 싸줘서 랜덤으로 첫 턴 걸리게끔

        clockUI = FindObjectOfType<UIClock>();
        for (int i = 0; i < MaxPlayerCount; i++)
        {
            orderPlayers[i].GetComponent<Player>();         
        }
        int RandomPlayerIndex = Random.Range(0, orderPlayers.Length);
        for (int i = 0; i < orderPlayers.Length;)
        {
            if (orderList.Contains(orderPlayers[RandomPlayerIndex]))
            {
                RandomPlayerIndex = Random.Range(0, orderPlayers.Length);
            }
            else
            {
                orderList.Add(orderPlayers[RandomPlayerIndex]);
                i++;
            }
            
        }

        
        OrderPlayer();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPlayer = orderList[currentTurnPlayer];
            orderList.RemoveAt(currentTurnPlayer);
            orderList.Add(currentPlayer);
            OrderPlayer();
            clockUI.currentTimeChange = clockUI.getMaxTime();
        }
        
    }
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
    /*public int CurrentTurnPlayer
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
    }*/
    void TurnEnd()
    {
        float currentTime = clockUI.currentTimeChange;
        if(currentTime <= 0)
        {
            currentPlayer = orderList[currentTurnPlayer];
            orderList.RemoveAt(currentTurnPlayer);
            orderList.Add(currentPlayer);
            clockUI.currentTimeChange = clockUI.getMaxTime();
        }
    }

    void ReverseTurn()
    {
        orderDirection = !orderDirection;
    }

    void OrderPlayer()
    {
        orderList[0].setPlayerState(myTurn);
        orderList[1].setPlayerState(NextTurn);
        orderList[2].setPlayerState(Wait);
        orderList[3].setPlayerState(Wait);
    }
    void NextCurrentTrunPlayer()
    {
        if(orderDirection == true)
        {

        }
    }
    void CheckTurn()
    {
        //리스트 인덱스에 따라 순서 정하기
    }
}
