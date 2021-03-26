using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 3.11 현재까지 턴매니저 기능
 * 플레이어 정보 받을 플레이어 배열 -> 들어온 플레이어들을 플레이어 리스트에 랜덤으로 쏴줌
 * -> 리스트 순서대로 플레이어들 상태를 0번째는 마이턴, 1번째는 넥스트턴, 2,3번째는 기다리는 중으로 설정
 * -> 턴이 끝났을 시(카드를 내거나(= 스페이스바로 임시 사용중)제한시간이 다되면 다음 플레이어 턴으로 넘어감
 * -> 리스트 0번째에 있는(마이턴) 플레이어를 currentPlayer에 넣어주고 리스트 0번째 삭제 -> 1,2,3번째 플레이어들 한칸씩 앞으로 떙겨짐(리스트는 가변적이라)
 * -> 이렇게되면 비어있는 3번째에 currentPlayer를 넣어주는 방식으로 턴을 돌리는 기능까지 완료 */
public class TurnManager : MonoBehaviour
{
    public int PlayerCount
    {
        get
        {
            return playerCount;
        }
        set
        {
            if (PlayerCount <= maxPlayerCount)
            {
                playerCount = value;
            }
        }
    }
    [SerializeField] private int maxPlayerCount = 4;
    
    [SerializeField] private List<Player> orderList = new List<Player>();  // 실질적인 턴을 결정함
    [SerializeField] private UIClock clockUI;

    private int playerCount = 0;
    private const int CURRENT_TURN_PLAYER_IDX = 0;//현재 턴인 플레이어 인덱스 -> List첫번쨰 플레이어가 턴이니깐 계속 0이면 될듯 // currentTurnPlayer
    private int reversCurrentTurnPlayer;
    private bool isOrderDirection = true; // true면 시계 방향

    private Player currentPlayer; // 현재 턴인 플레이어 받을 변수 -> 리스트0번쨰 플레이어가 계속 들어갈거임

    private ePlayerState myTurn = ePlayerState.myTurn;
    private ePlayerState nextTurn = ePlayerState.NextTurn;
    private ePlayerState wait = ePlayerState.Wait;


    private void Start()
    {
        
        //SetRandomOrderPlayers(GameManager.instance.Players);
    }
    private void Update()
    {
        QuitTurn();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOrderDirection == true)
            {
                ChangeOrderPlayer();
            }
            else
            {
                ChangeOrderRevers();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isOrderDirection = !isOrderDirection;
            SettingTurn();
        }
    }

    public void ChangeOrderPlayer() // 플레이어 순서 바꿔주기
    {
        SetCurrentPlayerAndRemoveList(CURRENT_TURN_PLAYER_IDX);
        orderList.Add(currentPlayer);
        currentPlayer = null;
        SettingTurn();
        clockUI.ResetCurrentTimeAndClockhand();
    }
    public void ChangeOrderRevers()//턴 순서 거꾸로
    {
        SetCurrentPlayerAndRemoveList(reversCurrentTurnPlayer);
        orderList.Insert(0, currentPlayer);
        currentPlayer = null;
        SettingTurn();
        clockUI.ResetCurrentTimeAndClockhand();
    }
    private void SetCurrentPlayerAndRemoveList(int index)
    {
        //@bug
        if(currentPlayer = null)
        {
            currentPlayer = orderList[index];
            orderList.RemoveAt(index);
        }
       
    }
    // Check 보다 Setting이 낫지 않을까? 
    private void SettingTurn()
    {
       
        if (isOrderDirection == true)
        {
            DecideOrder(orderList.Count, true);
        }
        else
        {
            DecideOrder(orderList.Count, false);
        }
    }
    private void DecideOrder(int count, bool direction) // 턴 방향에 따라 플레이어들 상태 정해주기
    {
        if (direction)
        {
            switch (count)
            {
                case 2:
                    orderList[0].PlayerState = myTurn;
                    orderList[1].PlayerState = nextTurn;
                    break;
                case 3:
                    orderList[0].PlayerState = myTurn;
                    orderList[1].PlayerState = nextTurn;
                    orderList[2].PlayerState = wait;
                    break;
                case 4:
                    orderList[0].PlayerState = myTurn;
                    orderList[1].PlayerState = nextTurn;
                    orderList[2].PlayerState = wait;
                    orderList[3].PlayerState = wait;
                    break;
                default:
                    Debug.Assert(false, "unknown type");
                    break;
            }
        }
        else
        {
            switch (orderList.Count)
            {
                case 2:
                    orderList[0].PlayerState = myTurn;
                    orderList[1].PlayerState = nextTurn;
                    break;
                case 3:
                    orderList[0].PlayerState = myTurn;
                    orderList[1].PlayerState = wait;
                    orderList[2].PlayerState = nextTurn;
                    break;
                case 4:
                    orderList[0].PlayerState = myTurn;
                    orderList[1].PlayerState = wait;
                    orderList[2].PlayerState = wait;
                    orderList[3].PlayerState = nextTurn;
                    break;
                default:
                    Debug.Assert(false, "unknown type");
                    break;
            }
        }
              
    }
    private void QuitTurn()
    {
        float currentTime = clockUI.CurrentTime;
        if (currentTime <= 0)
        {
            if (isOrderDirection == true)
            {
                ChangeOrderPlayer();
            }
            else
            {
                ChangeOrderRevers();
            }
        }
    }
  
    private void SetRandomOrderPlayers(List<Player> players)
    {
        int randomPlayerIndex = Random.Range(0, players.Count); //4명이면 0123
        for (int i = 0; i < players.Count; i++)
        {
            orderList.Add(players[randomPlayerIndex--]);
            if(randomPlayerIndex < 0)
            {
                randomPlayerIndex = players.Count - 1;
            }
        }
        reversCurrentTurnPlayer = players.Count - 1;
        SettingTurn();
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
    

    
    
    /*void NextCurrentTrunPlayer()
    {
        if(orderDirection == true)
        {

        }
    }*/
    
}

