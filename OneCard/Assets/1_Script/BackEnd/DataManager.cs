using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    public PlayerInfo CurrentPlayerInfo { get => currentPlayerInfo; set => currentPlayerInfo = value; }
    [SerializeField]
    PlayerInfo currentPlayerInfo;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
[System.Serializable]
public struct PlayerInfo
{
    public string nickname;
    public string uniqueID;
    public int level;
    public int tiket;
    public int rank;
    public int gold;
    public int diamond;

    public PlayerInfo(string nickname, string uniqueID, int level, int tiket, int rank, int gold, int diamond)
    {
        this.nickname = nickname;
        this.uniqueID = uniqueID;
        this.level = level;
        this.tiket = tiket;
        this.rank = rank;
        this.gold = gold;
        this.diamond = diamond;
    }

    public PlayerInfo(PlayerInfo playerInfo)
    {
        this.nickname = playerInfo.nickname;
        this.uniqueID = playerInfo.uniqueID;
        this.level = playerInfo.level;
        this.tiket = playerInfo.tiket;
        this.rank = playerInfo.rank;
        this.gold = playerInfo.gold;
        this.diamond = playerInfo.diamond;
    }

}
