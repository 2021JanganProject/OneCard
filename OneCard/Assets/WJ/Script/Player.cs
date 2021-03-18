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
public class Player : MonoBehaviour
{
    //플레이어 역할 / 플레이어 myTurn, 플레이어 ID, 턴에 따른 상태들, 상태에 따른 효과 정보
    public int PlayerID { get => playerID; set => playerID = value; }
    public string PlayerNickname { get => playerNickname; set => playerNickname = value; }
    public string PlayerRank { get => playerRank; set => playerRank = value; }
    public Image PlayerImage { get => playerImage; set => playerImage = value; }
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

  

    private int playerID;
    [SerializeField] private string playerNickname;
    [SerializeField] private string playerRank;
    [SerializeField] private Image playerImage;
    
    [SerializeField] private ePlayerState playerState;
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
