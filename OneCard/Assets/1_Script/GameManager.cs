using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// 역할 : 전체적인 게임의 설정과 관리를 담당

public enum eGameFlowState
{
    InitPlayer,
    InitGame,
    HandOutCards
}
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
    public int MaxPlayerCount { get => MaxPlayerCount; set => MaxPlayerCount = value; }
    /// <summary>
    /// PlayerActorNumber는 방에 들어온 순서대로 할당되는 플레이어 Number이다. Ex ) 첫번쨰로 들어오면 0  두번쨰로 들어오면 1
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
    /// <summary>
    ///  
    /// </summary>
    public int [] CurrentPlayerActorNumberArr
    {
        get
        {
            int[] playerListActorNum = new int[PhotonNetwork.PlayerList.Length];

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerListActorNum[i] = PhotonNetwork.PlayerList[i].ActorNumber-1;
            }
            return playerListActorNum;
        }
    }

    public GameObject[] PlayerObjArr { get => playerObjArr; set => playerObjArr = value; }
    public GameObject PlayerProfileBase { get => playerProfileBase;}
    public GameObject[] RemotePlayerObjArr { get => remotePlayerObjArr; set => remotePlayerObjArr = value; }

    [SerializeField] private CardManager cardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Transform [] spawnPositions;
    [SerializeField] private Transform localSpawnPosition;

    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject playerProfileBase;
    [SerializeField] private GameObject[] playerObjArr;

    [SerializeField] private GameObject localPlayerObj;
    [SerializeField] private GameObject[] remotePlayerObjArr;


    private int maxPlayerCount = 4;
    private int playerCount = 0;
    private int currentTurnPlayer;

    private bool isPlayerAllInTheRoom = false;
  
    

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        
       
    }
    
    IEnumerator Start()
    {
        playerObjArr = new GameObject[maxPlayerCount];
        remotePlayerObjArr = new GameObject[maxPlayerCount - 1];
        yield return new WaitForSeconds(1f);
        SpawnLocalPlayer();
        //SpawnPlayer();
        
        StartCoroutine(CoCheckingRoomFull());
    }
    IEnumerator CoCheckingRoomFull()
    {
        while(true)
        {
            if (IsTheRoomFull() && IsNotNullPlayerObjArr()) // 예외처리 : RPC로 Player들을 삽입 하기 때문에 들어오는 순서가 때마다 다름 
            {
                SetRemotePlayer();
                SetPlayersPosition();
                break;
            }
            yield return null;
        }
    }

    private void SetRemotePlayer()
    {
        
        
        List<GameObject> playerList = new List<GameObject>();
        for (int i = 0; i < maxPlayerCount; i++)
        {
            playerList.Add(playerObjArr[i]);
        }
        playerList.RemoveAt(LocalPlayerActorIndex);
        remotePlayerObjArr = playerList.ToArray();
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
    bool IsNotNullPlayerObjArr()
    {
        for (int i = 0; i < playerObjArr.Length; i++)
        {
            if (playerObjArr[i] == null)
            {
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
            
        }
    }

    void UpdatePlayerListLog()
    {
        DebugerManager.instance.ResetLog();
        DebugerManager.instance.Log($"totalplayer: {PhotonNetwork.PlayerList.Length}__playerList : {LocalPlayerActorIndex}");
       
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            DebugerManager.instance.Log($"_ {PhotonNetwork.PlayerList[i].ActorNumber-1}");
        }
    }


    
    /// <summary>
    ///  해당 역할은 TrunManager로 이동함
    /// </summary>
    public void TurnEnd()
    {
        //if (orderDirection)
        //{
        //    if (playerCount < 3)
        //    {
        //        playerCount += 1;
        //    }
        //    else
        //    {
        //        playerCount -= 3;
        //    }
        //}
        //else
        //{
        //    if (playerCount > 0)
        //    {
        //        playerCount -= 1;
        //    }
        //    else
        //    {
        //        playerCount += 3;
        //    }
        //}
        //orderTest.text = orderPlayers[playerCount].ToString();
    }

    public void ReverseTurn()
    {
        //orderDirection = !orderDirection;
    }


    private void NextCurrentTurnPlayer()
    {

    }

    [SerializeField] GameObject playerForTest;
    [SerializeField] PhotonView pvForTest;
    public GameObject baseForTest;
    private void SpawnPlayerForDebug()
    {
        GameObject playerGameObj = PhotonNetwork.Instantiate(playerForTest.name , new Vector3(Random.Range(-2f,2f),0,0),  Quaternion.Euler(new Vector3(0, 0, 0))); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
        pvForTest = playerGameObj.GetComponent<PhotonView>();
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

    private void OrderPlayer()
    {

    }



}
