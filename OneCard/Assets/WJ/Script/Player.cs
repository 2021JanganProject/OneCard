using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ePlayerState
{
    myTurn,
    NextTurn,
    Wait
}
public class Player : MonoBehaviourPunCallbacks
{
    //플레이어 역할 / 플레이어 myTurn, 플레이어 ID, 턴에 따른 상태들, 상태에 따른 효과 정보
    public string PlayerID { get => playerUniqueID;}
    /// <summary>
    /// 룸에 들어온 순서대로 ActorNumber가 정해진다. 
    /// </summary>
    public int PlayerActorNumber { get => playerActorNumber; }
    public string PlayerNickname { get => playerNickname;}
    public string PlayerRank { get => playerRank;}
    public Image PlayerImage { get => playerImage;}
    public ePlayerState PlayerState
    {
        get
        {
            return playerState;
        }
        set
        {
            playerState = value;
            DoSwitchPlayerState();
        }
    }

    private string playerUniqueID;
    [SerializeField] private int playerActorNumber;
    [SerializeField] private string playerNickname;
    [SerializeField] private string playerRank;
    [SerializeField] private Image playerImage;
    [SerializeField] private UIProfile uIProfile; 

    [SerializeField] private ePlayerState playerState;


    private void Start()
    {
        uIProfile.Playerstate = PlayerState;
    }
    private void Update()
    {
        uIProfile.Playerstate = PlayerState;
    }
   
    public void InitPlayer(PlayerInfo playerInfo , int ActorNumber)
    {
        playerNickname = playerInfo.nickname;
        playerRank = playerInfo.rank.ToString();
        playerUniqueID = playerInfo.uniqueID;
        playerActorNumber = ActorNumber;
        uIProfile.InitUIProfile(this);
    }

    public void DoSwitchPlayerState()
    {
        switch (playerState)
        {
            case ePlayerState.myTurn:
                Debug.Log("마이턴");
                break;
            case ePlayerState.NextTurn:
                Debug.Log("다음 차례");
                break;
            case ePlayerState.Wait:
                Debug.Log("기다리는중");
                break;
            default:
                Debug.Assert(false, "unknow type");
                break;
        }
    }
}
