using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "playerInfo/info")]
public class PlayerInfo : ScriptableObject
{
    public string PlayerName; //플레이어 이름

    
    public Sprite PlayerImage; // 플레이어 프로필 사진

    public Rank rank;
    

    

    public enum Rank
    {
       bronze,
       silver,
       gold,
       platinum,
       diamond,
       master,
       challenger
    }
}
