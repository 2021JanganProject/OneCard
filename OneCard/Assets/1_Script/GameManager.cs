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
    /// PlayerActorNumber는 방에 들어온 순서대로 할당되는 플레이어 Number이다. Ex ) 첫번쨰로 들어오면 0  두번쨰로 들어오면 1 ...
    /// </summary>
    public int LocalPlayerActorNumberStartZero
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

    public Transform LocalSpawnPositions { get => localSpawnPositions;}
    public GameObject[] PlayerArr { get => playerArr; set => playerArr = value; }
    public GameObject PlayerProfileBase { get => playerProfileBase;}

    [SerializeField] private CardManager cardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Transform [] spawnPositions;
    [SerializeField] private Transform localSpawnPositions;

    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject playerProfileBase;
    

    private int maxPlayerCount = 4;
    private int playerCount = 0;
    private int currentTurnPlayer;

    private bool isPlayerAllInTheRoom = false;
    //Text orderTest;
    [SerializeField] private GameObject[] playerArr;
    

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        
       
    }
    
    IEnumerator Start()
    {
        playerArr = new GameObject[4];
        yield return new WaitForSeconds(1f);
        SpawnLocalPlayer();
        //SpawnPlayer();
        
        StartCoroutine(Update1Sec());
    }
    IEnumerator Update1Sec()
    {
        while(maxPlayerCount <= PhotonNetwork.PlayerList.Length)
        {
            yield return new WaitForSeconds(1);
            Updatesdsd();
            UpdatePlayerListLog();
        }
    }
    void Updatesdsd()
    {
        
    }
   
    void UpdatePlayerListLog()
    {
        DebugerManager.instance.ResetLog();
        DebugerManager.instance.Log($"totalplayer: {PhotonNetwork.PlayerList.Length}__playerList : {LocalPlayerActorNumberStartZero}");
       
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
        
        var spawnPosition = spawnPositions[LocalPlayerActorNumberStartZero % spawnPositions.Length];

        

    }
    GameObject playerGameObj;
    [PunRPC]
    private void SpawnLocalPlayer()
    {
        Debug.Log($"localPlayerIndex :: {LocalPlayerActorNumberStartZero}");
        playerGameObj = PhotonNetwork.Instantiate(playerProfilePrefab.name, localSpawnPositions.position, localSpawnPositions.rotation); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
     
    }

    
    
    
    
    

    private void OrderPlayer()
    {

    }



}
