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
    /// 0번부터 시작하는 LocalPlayerActorNumber
    /// </summary>
    public int LocalPlayerActorNumberStartZero
    {
        get
        {
            return PhotonNetwork.LocalPlayer.ActorNumber - 1;
        }
    }
    [SerializeField] private Photon.Realtime.Player[] currentOrderPlayerArr;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Transform [] spawnPositions;
    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject playerProfileBase;
    [SerializeField] private Vector3 profileScale = new Vector3(2, 2, 2);

    private int maxPlayerCount = 4;
    private int playerCount = 0;
    private int currentTurnPlayer;
    private int[] orderPlayers = new int[4];

    private bool isPlayerAllInTheRoom =false;
    //Text orderTest;

    

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        
       
    }
    
    IEnumerator Start()
    {
       
        yield return new WaitForSeconds(1f);
        currentOrderPlayerArr = PhotonNetwork.PlayerList;
        SpawnPlayer();
        SpawnPlayerForDebug();
        StartCoroutine(Update1Sec());
    }
    IEnumerator Update1Sec()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            UpdatePlayerListLog();
        }
    }

    public int[] GetOrderPlayer()
    {
        int[] playerOrderNumberList = new int[currentOrderPlayerArr.Length];
        for (int i = 0; i < playerOrderNumberList.Length; i++)
        {
            playerOrderNumberList[i] = currentOrderPlayerArr[i].ActorNumber-1;
        }
        return playerOrderNumberList;
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
    GameObject playerGameObj;
    private void SpawnPlayerForDebug()
    {
        playerGameObj = PhotonNetwork.Instantiate(playerForTest.name , new Vector3(Random.Range(-2f,2f),0,0),  Quaternion.Euler(new Vector3(0, 0, 0))); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
        photonView.RPC("RPC_SetProfileBase", RpcTarget.All);
    }

    private void SpawnPlayer()
    {
        // LocalPlayer : 현재 방에 들어온 로컬 플레이어 (즉 나 자신)
        // 플레이어 번호를 가져온다.
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Debug.Log($"localPlayerIndex_{localPlayerIndex}");
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];
        GameObject playerGameObj =  PhotonNetwork.Instantiate(playerProfilePrefab.name, spawnPosition.position, spawnPosition.rotation ); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
        playerGameObj.transform.parent = playerProfileBase.transform;
        playerGameObj.transform.localScale = profileScale;
        Player player = playerGameObj.GetComponent<Player>();
        player.InitPlayer(DataManager.instance.CurrentPlayerInfo, LocalPlayerActorNumberStartZero);
    }
    [PunRPC]
    private void RPC_SetProfileBase(int parent)
    {
        playerGameObj.transform.parent = playerProfileBase.transform;
    }
    


    private void OrderPlayer()
    {

    }



}
