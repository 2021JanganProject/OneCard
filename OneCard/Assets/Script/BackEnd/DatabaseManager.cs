using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;

// 최초 로그인 시에만 PlayerInfo 개체를 생성해서 서버에 전송하고 싶다. 
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public const string FIREBASE_PATH = "https://one-card-backend-default-rtdb.firebaseio.com/";
    public FirebaseDatabase firebaseDatabase { get; set; }
    public DatabaseReference reference { get; set; }
    public PlayerInfo PlayerInfo { get => playerInfo; set => playerInfo = value; }

    private PlayerInfo playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        DebugerManager.instance.del_debugInputR += AsyncGetPlayerInfo;
        DebugerManager.instance.del_debugInputT += () => Debug.Log(playerInfo.uniqueID); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitFirstLoginUserDatabase()
    {
        InitGuestPlayerInfo();
        
        Debug.Log("Set");
    }
    // ghj
    void InitGuestPlayerInfo()
    {
        PlayerInfo playerInfo = new PlayerInfo("Guest", AuthManager.instance.GetUserId(), 1, 5, 1, 100, 0);
        string json = JsonUtility.ToJson(playerInfo);
        PushPlayerInfoToDB(json);
    }

    void PushPlayerInfoToDB(string json )
    {
        reference = firebaseDatabase.RootReference;
        Debug.Log(firebaseDatabase.RootReference);
        string key = reference.Child("PlayerInfo").Push().Key;
        reference.Child("PlayerInfo").Child(key).SetRawJsonValueAsync(json);
    }

    void GetPlayerInfo(DataSnapshot item)
    {
        IDictionary playerInfoTemp = (IDictionary)item.Value;
        Debug.Log($"DB ID :{playerInfoTemp["uniqueID"].ToString()} , USER ID  : {AuthManager.instance.GetUserId()}");
        if (playerInfoTemp["uniqueID"].ToString() == AuthManager.instance.GetUserId())
        {
            Debug.Log("Find!");

            playerInfo = new PlayerInfo
            (
                playerInfoTemp["nickname"].ToString(),
                playerInfoTemp["uniqueID"].ToString(),
                int.Parse(playerInfoTemp["level"].ToString()),
                int.Parse(playerInfoTemp["tiket"].ToString()),
                int.Parse(playerInfoTemp["rank"].ToString()),
                int.Parse(playerInfoTemp["gold"].ToString()),
                int.Parse(playerInfoTemp["diamond"].ToString())
            );
        }
    }

    async void AsyncGetPlayerInfo()
    {
        DatabaseReference reference = firebaseDatabase.GetReference("PlayerInfo");
        
        await reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var item in snapshot.Children)
                {
                    IDictionary playerInfoTemp = (IDictionary)item.Value;
                    Debug.Log($"DB ID :{playerInfoTemp["uniqueID"].ToString()} , USER ID  : {AuthManager.instance.GetUserId()}");
                    if (playerInfoTemp["uniqueID"].ToString() == AuthManager.instance.GetUserId())
                    {
                        Debug.Log("Find!");
                        //selectdPlayerInfoTemp = playerInfoTemp;
                        GetPlayerInfo(item);
                    }
                }
            }
        });
        
    }
}

public struct PlayerInfo
{
    public string nickname;
    public string uniqueID;
    public int level;
    public int tiket;
    public int rank;
    public int gold;
    public int diamond;

    public PlayerInfo(string nickname, string uniqueID , int level, int tiket, int rank, int gold, int diamond)
    {
        this.nickname = nickname;
        this.uniqueID = uniqueID;
        this.level = level;
        this.tiket = tiket;
        this.rank = rank;
        this.gold = gold;
        this.diamond = diamond;
    }

}
