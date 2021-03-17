using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private CardManager cardManager;
    [SerializeField] private bool orderDirection = true;
    [SerializeField] private Transform [] spawnPositions;
    [SerializeField] private GameObject playerProfilePrefab;
    [SerializeField] private GameObject playerProfileBase;

    private int MaxPlayerCount;
    private int playerCount = 0;
    private int currentTurnPlayer;
    private int[] orderPlayers = new int[4];
    [SerializeField] Text orderTest;

    private void Start()
    {
        SpawnPlayer();
        orderPlayers[0] = 1;
        orderPlayers[1] = 2;
        orderPlayers[2] = 3;
        orderPlayers[3] = 4;
    }
    public void TurnEnd()
    {
        if(orderDirection)
        {
            if(playerCount < 3)
            {
                playerCount += 1;
            }
            else
            {
                playerCount -= 3;
            }
        }
        else
        {
            if (playerCount > 0)
            {
                playerCount -= 1;
            }
            else
            {
                playerCount += 3;
            }
        }
        orderTest.text = orderPlayers[playerCount].ToString();
    }

    public void ReverseTurn()
    {
        orderDirection = !orderDirection;
    }


    private void NextCurrentTurnPlayer()
    {

    }

    private void SpawnPlayer()
    {
        // LocalPlayer : 현재 방에 들어온 로컬 플레이어 (즉 나 자신)
        // 플레이어 번호를 가져온다.
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Debug.Log($"localPlayerIndex{localPlayerIndex}");
        //var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];
        //// GameObject.Instantiate() 로컬 세상에서만 생성됨
        //GameObject profile =  PhotonNetwork.Instantiate(playerProfilePrefab.name, spawnPosition.position, spawnPosition.rotation); // 리소스에서 이름값으로 가져옴. 알아서 동기화를 해준다. 
        //profile.transform.parent = playerProfileBase.transform;
    }


    private void OrderPlayer()
    {

    }



}
