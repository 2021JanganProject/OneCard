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
    //서버에서 플레이어 데이터를 받아와서 배열에 넣어줌
    [SerializeField]
    private static int MaxPlayerCount = 4;

    private Player currentPlayer; // 현재 턴인 플레이어 받을 변수 -> 리스트0번쨰 플레이어가 계속 들어갈거임

    private int playerCount = 0;
    private int currentTurnPlayer = 0;//현재 턴인 플레이어 인덱스 -> List첫번쨰 플레이어가 턴이니깐 계속 0이면 될듯
    private int ReversCurrentTurnPlayer;
    [SerializeField]
    private Player[] orderPlayers; // 들어온 플레이어 보관함
    [SerializeField]
    private List<Player> orderList = new List<Player>();  // 실질적인 턴을 결정함
    private bool orderDirection = true; // true면 시계 방향
    

    private UIClock clockUI;

    private PlayerState myTurn = PlayerState.myTurn;
    private PlayerState NextTurn = PlayerState.NextTurn;
    private PlayerState Wait = PlayerState.Wait;

    public Player[] GetOrderPlaeyrs() // 지워도 될 듯. 받아온 플레이어 정보를 다른 클래스에서 알 필요가 있을까? UI프로필 관리를 한 클래스에서만 하면 필요할수도
    {
        return orderPlayers;
    }
    public List<Player> GetOrderList() // 지워도 될 듯. 플레이어 턴 관리 리스트인데 턴매니저 말고 접근을 할까?
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
        
        for(int i = 0; i < orderPlayers.Length; i++)
        {
            orderList.Add(orderPlayers[RandomPlayerIndex--]);
            if (RandomPlayerIndex < 0)
                RandomPlayerIndex = orderPlayers.Length-1;
            
        }

        ReversCurrentTurnPlayer = orderPlayers.Length-1;

        
        OrderPlayer();
        
    }
    private void Update()
    {
        TurnEnd();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (orderDirection == true)
            {
                OrderPlayer();
            }
            else
            {
                ReverseTurn();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            orderDirection = !orderDirection;
            CheckTurn();
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
    private void TurnEnd()
    {
        float currentTime = clockUI.currentTimeChange;
        if(currentTime <= 0)
        {
            if(orderDirection == true)
            {
                
                OrderPlayer();
            }
            else
            {
                
                ReverseTurn();
            }
        }
    }

    public void ReverseTurn()//턴 순서 거꾸로
    {
        //orderDirection = !orderDirection;
        SetCurrentPlayerAndRemoveList(ReversCurrentTurnPlayer);
        //currentPlayer = orderList[ReversCurrentTurnPlayer];
        //orderList.RemoveAt(ReversCurrentTurnPlayer);
        orderList.Insert(0, currentPlayer);
        CheckTurn();
        clockUI.resetCurrentTimeAndClockhand();

    }
    private void SetCurrentPlayerAndRemoveList(int index)
    {
        currentPlayer = orderList[index];
        orderList.RemoveAt(index);
    }
    void OrderPlayer() // 플레이어 순서 바꿔주기
    {
        SetCurrentPlayerAndRemoveList(currentTurnPlayer);
        //currentPlayer = orderList[currentTurnPlayer];
        //orderList.RemoveAt(currentTurnPlayer);
        orderList.Add(currentPlayer);
        CheckTurn();
        clockUI.resetCurrentTimeAndClockhand();
    }
    /*void NextCurrentTrunPlayer()
    {
        if(orderDirection == true)
        {

        }
    }*/
    void CheckTurn()
    {
        //리스트 인덱스에 따라 순서 정하기
        if(orderDirection == true)
        {
            orderList[0].setPlayerState(myTurn);
            orderList[1].setPlayerState(NextTurn);
            orderList[2].setPlayerState(Wait);
            orderList[3].setPlayerState(Wait);
        }
        else
        {
            orderList[0].setPlayerState(myTurn);
            orderList[1].setPlayerState(Wait);
            orderList[2].setPlayerState(Wait);
            orderList[3].setPlayerState(NextTurn);
        }
       

        
    }
}

