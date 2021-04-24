using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

/* 3.11 현재까지 턴매니저 기능
 * 플레이어 정보 받을 플레이어 배열 -> 들어온 플레이어들을 플레이어 리스트에 랜덤으로 쏴줌
 * -> 리스트 순서대로 플레이어들 상태를 0번째는 마이턴, 1번째는 넥스트턴, 2,3번째는 기다리는 중으로 설정
 * -> 턴이 끝났을 시(카드를 내거나(= 스페이스바로 임시 사용중)제한시간이 다되면 다음 플레이어 턴으로 넘어감
 * -> 리스트 0번째에 있는(마이턴) 플레이어를 currentPlayer에 넣어주고 리스트 0번째 삭제 -> 1,2,3번째 플레이어들 한칸씩 앞으로 떙겨짐(리스트는 가변적이라)
 * -> 이렇게되면 비어있는 3번째에 currentPlayer를 넣어주는 방식으로 턴을 돌리는 기능까지 완료 */
public class TurnManager : MonoBehaviourPun
{
    public static TurnManager instance;
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

    public int CurrentTurnIdx { get => CurrentTurnPlayer.PlayerActorIndex; }
    public Player CurrentTurnPlayer { get => orderList[0]; set => orderList[0] = value; } // 현재 턴인 플레이어 받을 변수 -> 리스트0번쨰 플레이어가 계속 들어갈거임
    public List<Player> OrderList { get => orderList; set => orderList = value; }

    private const int CURRENT_TURN_PLAYER_IDX = 0;//현재 턴인 플레이어 인덱스 -> List첫번쨰 플레이어가 턴이니깐 계속 0이면 될듯 

    [SerializeField] private int maxPlayerCount = 4;
    [SerializeField] private List<Player> orderList = new List<Player>();  // 실질적인 턴을 결정함
    [SerializeField] private UIClock clockUI;

    private int playerCount = 0;
    private int reversCurrentTurnPlayer;
    private int currentTurnIdx;
    private bool isOrderDirection = true; // true면 시계 방향


    private ePlayerState myTurn = ePlayerState.myTurn;
    private ePlayerState nextTurn = ePlayerState.NextTurn;
    private ePlayerState wait = ePlayerState.Wait;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
      
    }
    void UpdatePlayers()
    {
        for (int i = 0; i < orderList.Count; i++)
        {
            GameManager.instance.playersActorNumDebug[i].text = $"Player : {orderList[i].PlayerActorIndex.ToString()}";
        }
    }

    public void RPC_ALL_EndTurn()
    {
        if (isOrderDirection == true)
        {
            RPC_ALL_ChangeOrderPlayer();
        }
        else
        {
            ChangeOrderRevers();
        }
    }
  
    private void Update()
    {
        
        QuitTurn();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            RPC_ALL_EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isOrderDirection = !isOrderDirection;
            SettingTurn();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(CoUpdatePlayers());
        }
        

    }
    IEnumerator CoUpdatePlayers()
    {
        while (true)
        {
            UpdatePlayers();
            photonView.RPC(nameof(GameManager.UpdateCurrentTurnUIForDebug), RpcTarget.All);
            yield return null;
        }
    }

    public void RPC_ALL_ChangeOrderPlayer()
    {
        photonView.RPC(nameof(ChangeOrderPlayer), RpcTarget.All);
    }
    [PunRPC]
    private void ChangeOrderPlayer() // 플레이어 순서 바꿔주기 
    {
        
        Player currentTurnPlayer = CurrentTurnPlayer;
        orderList.RemoveAt(CURRENT_TURN_PLAYER_IDX);
        orderList.Add(currentTurnPlayer);
        SettingTurn();
        clockUI.ResetCurrentTimeAndClockhand();
    }
    public void ChangeOrderRevers()//턴 순서 거꾸로
    {
        SetCurrentPlayerAndRemoveList(reversCurrentTurnPlayer);
        orderList.Insert(0, CurrentTurnPlayer);
        SettingTurn();
        clockUI.ResetCurrentTimeAndClockhand();
    }
    public bool IsMyturn()
    {
        Debug.Log($"GameManager.instance.LocalPlayerActorIndex_{GameManager.instance.LocalPlayerActorIndex } == CurrentTurnIdx_{CurrentTurnIdx}");
        if (GameManager.instance.LocalPlayerActorIndex == CurrentTurnIdx)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetCurrentPlayerAndRemoveList(int index)
    {
        if(CurrentTurnPlayer == null)
        {
            CurrentTurnPlayer = orderList[index];
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
                case 1:
                    orderList[0].PlayerState = myTurn;
                    break;
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
                case 1:
                    orderList[0].PlayerState = myTurn;
                    break;
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
    public IEnumerator CoSetRandomOrderPlayersArrToList()
    {
        while(true)
        {
            if (GameManager.instance.RandomPlayerIndex < 0)
            {
                Debug.Log("Waiting RandomPlayerIndex...");
                yield return null;
                continue;
            }
            SetRandomOrderPlayers(GameManager.instance.RandomPlayerIndex);
            yield return null;
            break;
        }
    }
    [PunRPC]
    public void SetRandomOrderPlayers(int randomPlayerIndex)
    {
        // #Arr => List
        List<Player> playerList = new List<Player>();
        for (int i = 0; i < GameManager.instance.PlayerArr.Length; i++)
        {
            playerList.Add(GameManager.instance.PlayerArr[i]);
        }
        // # Sorting

        List<Player> sortedPlayerList = playerList.OrderBy(x => x.PlayerActorIndex).ToList();
        foreach (var item in sortedPlayerList)
        {
            DebugGUI.Log_White(item.transform.name);
        }
        playerList = sortedPlayerList;
        // # Logic
        for (int i = 0; i < playerList.Count; i++)
        {
            Debug.Log(randomPlayerIndex);
            orderList.Add(playerList[randomPlayerIndex]);
            randomPlayerIndex--;
            if (randomPlayerIndex < 0)
            {
                randomPlayerIndex = playerList.Count - 1;
            }
        }
        reversCurrentTurnPlayer = playerList.Count - 1;
        SettingTurn();

        //SetRandomOrderPlayers(playerList, randomPlayerIndex);
    }


    private void SetRandomOrderPlayers(List<Player> players , int randomPlayerIndex)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log(randomPlayerIndex);
            orderList.Add(players[randomPlayerIndex--]);
            if (randomPlayerIndex < 0)
            {
                randomPlayerIndex = players.Count - 1;
            }
        }
        reversCurrentTurnPlayer = players.Count - 1;
    }
}

