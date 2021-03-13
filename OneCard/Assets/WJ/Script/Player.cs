using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    myTurn,
    NextTurn,
    Wait
}
public class Player : MonoBehaviour
{
    //플레이어 역할 / 플레이어 myTurn, 플레이어 ID, 턴에 따른 상태들, 상태에 따른 효과 정보
    
    private int PlayerID;
    [SerializeField]
    private string PlayerNickname;
    [SerializeField]
    private string PlayerRank;
    [SerializeField]
    private Image PlayerImage;

    private Image turnStateImage;
   
    [SerializeField]
    private PlayerState playerState;
    
    
    
    public string getPlayerRank()
    {
        return PlayerRank;
    }
    public string getPlayerNickname()
    {
        return PlayerNickname;
    }
    public Image getPlayerImage()
    {
        return PlayerImage;
    }
    
    public PlayerState getPlayerState()
    {
        return playerState;
    }
    public void setPlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
        SwitchPlayerState();
    }
    public void SwitchPlayerState()
    {
        switch (playerState)
        {
            case PlayerState.myTurn:
                Debug.Log("마이턴");
                setMyTurn();
                break;
            case PlayerState.NextTurn:
                Debug.Log("다음 차례");
                break;
            case PlayerState.Wait:
                Debug.Log("기다리는중");
                break;
        }
    }
    public void setPlayerID(int ID)
    {
        this.PlayerID = ID;
    }
    public void setPlayerImage(Image image)
    {
        this.PlayerImage = image;
    }


    public void setMyTurn()
    {
            
    }
}
