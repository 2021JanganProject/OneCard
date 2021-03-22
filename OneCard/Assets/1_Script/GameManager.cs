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

    [SerializeField] private CardManager cardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Transform [] spawnPositions;
    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject playerProfileBase;

    private int maxPlayerCount = 4;
    private int playerCount = 0;
    private int currentTurnPlayer;
    private int[] orderPlayers = new int[4];

    private bool isPlayerAllInTheRoom =false;
    [SerializeField] Text orderTest;

    

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
        SpawnPlayer();
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

    void UpdatePlayerListLog()
    {
        
        DebugerManager.instance.ResetLog();
        DebugerManager.instance.Log($"totalplayer: {PhotonNetwork.PlayerList.Length}__playerList : {PhotonNetwork.LocalPlayer.ActorNumber}");
       
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            DebugerManager.instance.Log($"_ {PhotonNetwork.PlayerList[i].ActorNumber}");
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

    [SerializeField]GameObject playerForTest;
    private void SpawnPlayer()
    {
        // LocalPlayer : 현재 방에 들어온 로컬 플레이어 (즉 나 자신)
        // 플레이어 번호를 가져온다.
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Debug.Log($"localPlayerIndex_{localPlayerIndex}");
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];
        //// GameObject.Instantiate() 로컬 세상에서만 생성됨
        playerForTest =  PhotonNetwork.Instantiate(playerProfilePrefab.name, spawnPosition.position, spawnPosition.rotation ); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
        playerForTest.transform.parent = playerProfileBase.transform;
        playerForTest.transform.localScale = new Vector3(1, 1, 1);

        
    }


    private void OrderPlayer()
    {

    }



}
