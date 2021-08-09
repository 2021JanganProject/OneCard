using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
// 역할 : 전체적인 게임의 설정과 관리를 담당
public enum eGameFlowState
{
    WaittingPlayer_0,
    InitPlayer_1,
    InitCard_2,
    InitGame_3,
    HandOutCards,
    DEFAULT
}
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
    public int MaxPlayerCount { get => maxPlayerCount; set => maxPlayerCount = value; }
    /// <summary>
    /// 현재 나의 PlayerActorNumberIndex이다. PlayerActorNumber는 방에 들어온 순서대로 할당되는 플레이어 Number이다. 
    /// </summary>
    public int LocalPlayerActorIndex
    {
        get
        {
            int actorNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            Debug.Log($"LocalActorNumber :: {actorNum}");
            return actorNum;
        }
    }
    public GameObject[] PlayerObjArr { get => playerObjArr; set => playerObjArr = value; } // 체크 필요 
    public GameObject PlayerProfileBase { get => playerProfileBase;}
    /// <summary>
    /// 나를 제외한 다른 플레이어들의 
    /// </summary>
    public GameObject[] RemotePlayerObjArr { get => remotePlayerObjArr; set => remotePlayerObjArr = value; }
    /// <summary>
    /// 나의 PlayerObj
    /// </summary>
    public GameObject LocalPlayerObj { get => localPlayerObj; set => localPlayerObj = value; }
    public Player[] PlayerArr { get => playerArr; set => playerArr = value; }
    public int RandomPlayerIndex { get => randomPlayerIndex; set => randomPlayerIndex = value; }

    public eGameFlowState gameFlowState = eGameFlowState.WaittingPlayer_0;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Transform [] spawnPositions;
    [SerializeField] private Transform localSpawnPosition;

    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject playerProfileBase;
    [SerializeField] private GameObject[] playerObjArr; // GameObj
    [SerializeField] private Player[] playerArr; // Player

    [SerializeField] private GameObject localPlayerObj;
    [SerializeField] private GameObject[] remotePlayerObjArr;

    [SerializeField] Text currentTurnPlayerTextForDebug, currentTurnIndexTextForDebug;
    [SerializeField] public Text [] playersActorNumDebug;

    [SerializeField] public DebugGUI debugGUI;


    private int maxPlayerCount = 4;
    private int playerCount = 0;
    [SerializeField] private int randomPlayerIndex = -1;
    private int currentTurnPlayer;
 
    private bool isPlayerAllInTheRoom = false;

    [SerializeField] private GameObject TimerPrefab;
    [SerializeField] private Canvas canvas;
  
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    IEnumerator Start()
    {
        #region ==배열 초기화==
        playerObjArr = new GameObject[maxPlayerCount];
        playerArr = new Player[maxPlayerCount];
        remotePlayerObjArr = new GameObject[maxPlayerCount - 1];
        #endregion
        SpawnLocalPlayer();
        yield return new WaitForSeconds(0.2f);
        UpdateGameState(eGameFlowState.WaittingPlayer_0);

        if (PhotonNetwork.IsMasterClient == true)
        {
            CardManager.instance.SettingCard();
        }
        else if(PhotonNetwork.IsMasterClient == false)
        {
            CardManager.instance.AddCloseCards();
        }
              
    }

    bool await = false;
    // 플레이어 대기
    // 
    void UpdateGameState(eGameFlowState chageGameFlowState)
    {
        gameFlowState = chageGameFlowState;
        Debug.Log($"GameFLow ::{gameFlowState}");
        switch (gameFlowState)
        {
            case eGameFlowState.WaittingPlayer_0:
                //GameState(eGameFlowState.InitPlayer_1);
                StartCoroutine(CoCheckingRoomFull());
                break;
            case eGameFlowState.InitCard_2:
                break;
            case eGameFlowState.InitGame_3:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DebugGUI.Log_White($"Remote randomPlayerIndex : {randomPlayerIndex}");
        }
    }
    private void InitOfflineDataForDebug()
    {
        SetPlayerObjArrAndRemotePlayerObjArr();
        SetPlayerArr();
        //SetOrderList();
    }
   

    IEnumerator CoCheckingRoomFull()
    {
        while(true)
        {
            if (IsTheRoomFull() == false) // 예외처리 : RPC로 Player들을 삽입 하기 때문에 들어오는 순서가 때마다 다름  
            {
                Debug.Log("Waiting... RoomFull");
                yield return null;
                continue;
            }
            if(IsNotNullPlayerObjArr() == false)
            {
                Debug.Log("Waiting...IsNotNullPlayerObjArr");
                yield return null;
                continue;
            }

            SetPlayerObjArrAndRemotePlayerObjArr();
            SetPlayerArr();
            SetPlayersPosition();            
            
            RPC_M_SetRandomPlayerIndex();
            
            // 시간 동기화를 위해 타이머 생성
            var timer = PhotonNetwork.Instantiate(TimerPrefab.name, canvas.transform.position, canvas.transform.rotation).transform.parent = canvas.transform;
            timer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
            //timer.localScale = new Vector3(1, 1, 0);

            DebugGUI.Log_White($"Remote randomPlayerIndex : {randomPlayerIndex}");
            StartCoroutine(TurnManager.instance.CoSetRandomOrderPlayersArrToList());
            CardManager.instance.RPC_M_FirstOpenCard();
            //TurnManager.instance.CoSetRandomOrderPlayersArrToList(randomPlayerIndex);
            yield return null;
            break;
        }
    }
    
    private void SetPlayerObjArrAndRemotePlayerObjArr()
    {
        // 플레이어 오브젝트 찾아서 삽입 
        playerObjArr = GameObject.FindGameObjectsWithTag("Player");

        List<GameObject> remotePlayerObjListTemp = new List<GameObject>();
        // RemotePlayer 오브젝트 삽입 로직
        for (int i = 0; i < playerObjArr.Length; i++)
        {
            if (playerObjArr[i].GetComponent<Player>().PlayerActorIndex != LocalPlayerActorIndex)
            {
                Debug.Log($"{i}_PlayerActorIndex_{playerObjArr[i].GetComponent<Player>().PlayerActorIndex} == LocalPlayerActorIndex_{LocalPlayerActorIndex}");
                remotePlayerObjListTemp.Add(playerObjArr[i]);
            }
        }
        remotePlayerObjArr = remotePlayerObjListTemp.ToArray();
    }
    private void SetPlayerArr()
    {
        for (int i = 0; i < playerObjArr.Length; i++)
        {
            playerArr[i] =  playerObjArr[i].GetComponent<Player>();
        }
    }
    void RPC_M_SetRandomPlayerIndex()
    {
        if (photonView.IsMine && PhotonNetwork.IsMasterClient) // 일단 남겨둠
        {
            photonView.RPC(nameof(SetRandomPlayerIndex), RpcTarget.MasterClient);
        }
    }
   
    [PunRPC]
    private void SetRandomPlayerIndex() // 마스터만 하고 나머지 값은 참조 해야함
    {
        int randomPlayerIndex = UnityEngine.Random.Range(0, playerArr.Length);
        DebugGUI.Log_White($"Master RandomPlayerIndex : {randomPlayerIndex}");
        photonView.RPC(nameof(SendRandomPlayerIndex), RpcTarget.AllViaServer , randomPlayerIndex);
    }
    [PunRPC]
    private void SendRandomPlayerIndex(int randomPlayerIndex)
    {
        this.randomPlayerIndex = randomPlayerIndex;
    }
    bool IsTheRoomFull()
    {
        if(maxPlayerCount <= PhotonNetwork.PlayerList.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //PlayerObjArr
    bool IsNotNullPlayerObjArr()
    {
        for (int i = 0; i < playerObjArr.Length; i++)
        {
            if (playerObjArr[i] == null)
            {
                Debug.Log($"Waiting... playerObjArr[{i}] is NULL");
                return false;
            }
        }
        return true;
        
    }


    void SetPlayersPosition()
    {
        // 로컬 먼저 배치
        localPlayerObj.transform.position = localSpawnPosition.position;
        int startRemoteIndex = LocalPlayerActorIndex-1;
        // 0번째 자신의 인덱스 + 1
        // 만약 넘어가면 다시 0부터 시작
        for (int i = 0; i < remotePlayerObjArr.Length; i++)
        {
            startRemoteIndex++;
            if (remotePlayerObjArr.Length <= startRemoteIndex)
            {
                startRemoteIndex = 0;
            }
            Debug.Log($"startRemoteIndex{startRemoteIndex}");
            GameObject player = remotePlayerObjArr[startRemoteIndex];
            player.transform.position = spawnPositions[i].position;
            player.GetComponent<Player>().CardHandPos = CardManager.instance.RemoteCardPosArr[i];

        }
    }

    [PunRPC]
    private void RPC_SpawnPlayer()
    {
        // LocalPlayer : 현재 방에 들어온 로컬 플레이어 (즉 나 자신)
        // 플레이어 번호를 가져온다.
        
        var spawnPosition = spawnPositions[LocalPlayerActorIndex % spawnPositions.Length];
    }
    
    [PunRPC]
    private void SpawnLocalPlayer()
    {
        Debug.Log($"localPlayerIndex :: {LocalPlayerActorIndex}");
        localPlayerObj = PhotonNetwork.Instantiate(playerProfilePrefab.name, localSpawnPosition.position, localSpawnPosition.rotation); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
    }


    [PunRPC]
    public void UpdateCurrentTurnUIForDebug()
    {
        currentTurnIndexTextForDebug.text = $"현재 턴 {TurnManager.instance.CurrentTurnIdx}";
        currentTurnPlayerTextForDebug.text = $"현재 플레이어 ID {localPlayerObj.GetComponent<Player>().PlayerActorIndex}";
    }
}
