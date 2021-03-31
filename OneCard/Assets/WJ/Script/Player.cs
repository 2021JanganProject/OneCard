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
    /// 룸에 들어온 순서대로 ActorIndex가 정해진다. ActorNum - 1 을 한 값
    /// </summary>
    public int PlayerActorIndex { get => playerActorNumber; }
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
    [SerializeField] private Vector3 profileScale = new Vector3(2, 2, 2);

    private void Start()
    {
        uIProfile.Playerstate = PlayerState;
        photonView.RPC("RPC_SetTransform", RpcTarget.All);
        photonView.RPC("RPC_InitPlayer", RpcTarget.All);
        photonView.RPC("RPC_SetPlayerArr", RpcTarget.All);
    }
    private void Update()
    {
        uIProfile.Playerstate = PlayerState;
    }
   
    
    [PunRPC]
    private void RPC_InitPlayer()
    {
        InitPlayer(DataManager.instance.CurrentPlayerInfo, photonView.Controller.ActorNumber-1);
    }
    private void InitPlayer(PlayerInfo playerInfo, int ActorNumber)
    {
        playerNickname = playerInfo.nickname;
        playerRank = playerInfo.rank.ToString();
        playerUniqueID = playerInfo.uniqueID;
        playerActorNumber = ActorNumber;
        uIProfile.InitUIProfile(this);
    }
    [PunRPC]
    private void RPC_SetTransform()
    {
        SetTransform();
    }
    private void SetTransform() // 부모 , 크기 , 이름
    {
        transform.parent = GameManager.instance.PlayerProfileBase.transform;
        transform.localScale = profileScale;
        transform.name = $"Player_{photonView.Controller.ActorNumber}";
    }
    [PunRPC]
    private void RPC_SetPlayerArr()
    {
        SetPlayerArr();
    }
    private void SetPlayerArr()
    {
        GameManager.instance.PlayerObjArr[PlayerActorIndex] = gameObject;
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
