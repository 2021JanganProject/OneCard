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
            if (PlayerCount <= MaxPlayerCount)
            {
                playerCount = value;
            }
        }
    }
    [SerializeField] private static int MaxPlayerCount = 4;
    
    [SerializeField] private List<Player> orderList = new List<Player>();  // 실질적인 턴을 결정함
    [SerializeField] private UIClock clockUI;

    private int playerCount = 0;
    private const int CURRENT_TURN_PLAYER_IDX = 0;//현재 턴인 플레이어 인덱스 -> List첫번쨰 플레이어가 턴이니깐 계속 0이면 될듯 
    private int ReversCurrentTurnPlayer;
    private bool isOrderDirection = true; // true면 시계 방향

    private Player currentPlayer; // 현재 턴인 플레이어 받을 변수 -> 리스트0번쨰 플레이어가 계속 들어갈거임

    private ePlayerState myTurn = ePlayerState.myTurn;
    private ePlayerState NextTurn = ePlayerState.NextTurn;
    private ePlayerState Wait = ePlayerState.Wait;


    private void Start()
    {
        //SetRandomOrderPlayers(GameManager.instance.GetOrderPlayer());
    }

    public void ChangeOrderPlayer() // 플레이어 순서 바꿔주기
    {
        SetCurrentPlayerAndRemoveList(CURRENT_TURN_PLAYER_IDX);
        orderList.Add(currentPlayer);
        CheckTurn();
        clockUI.ResetCurrentTimeAndClockhand();
    }
    public void ChangeOrderReverse()//턴 순서 거꾸로
    {
        SetCurrentPlayerAndRemoveList(ReversCurrentTurnPlayer);
        orderList.Insert(0, currentPlayer);
        CheckTurn();
        clockUI.ResetCurrentTimeAndClockhand();
    }
    private void SetCurrentPlayerAndRemoveList(int index)
    {
        currentPlayer = orderList[index];
        orderList.RemoveAt(index);
    }
    // Check 보다 Setting이 낫지 않을까? 
    private void CheckTurn()
    {
        //리스트 인덱스에 따라 순서 정하기
        if (isOrderDirection == true)
        {
            orderList[0].PlayerState = myTurn;
            orderList[1].PlayerState = NextTurn;
            orderList[2].PlayerState = Wait;
            orderList[3].PlayerState = Wait;
            // 0 번 자기턴 
            // 1 번 넥스트 턴
            // Default Wait 
        }
        else
        {
            orderList[0].PlayerState = myTurn;
            orderList[1].PlayerState = Wait;
            orderList[2].PlayerState = Wait;
            orderList[3].PlayerState = NextTurn;
            // 0번 자기턴
            // (방에 들어온 플레이어 수 - 1 ) 번 넥스트 턴
            // Defalut Wait 
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
                ChangeOrderReverse();
            }
        }
    }
    private void Awake()
    {

    }

    private void SetRandomOrderPlayers(List<Player> players)
    {
        int RandomPlayerIndex = Random.Range(0, players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            orderList.Add(orderList[RandomPlayerIndex--]);
            if(RandomPlayerIndex < 0)
            {
                RandomPlayerIndex = players.Count - 1;
            }
        }
        ReversCurrentTurnPlayer = players.Count - 1;
        CheckTurn();
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
                ChangeOrderReverse();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isOrderDirection = !isOrderDirection;
            CheckTurn();
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
    

    
    
    /*void NextCurrentTrunPlayer()
    {
        if(orderDirection == true)
        {

        }
    }*/
    
}

