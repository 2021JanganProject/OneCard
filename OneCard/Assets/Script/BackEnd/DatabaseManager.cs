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
        DebugerManager.instance.del_debugInputR += GetPlayerInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitUserDatabase()
    {
        InitPlayerInfo();
        
        Debug.Log("Set");
    }
    // ghj
    void InitPlayerInfo()
    {
        PlayerInfo playerInfo = new PlayerInfo("Test3", AuthManager.instance.GetUserId(), 1, 5, 4, 4, 4);
        string json = JsonUtility.ToJson(playerInfo);
        reference = firebaseDatabase.RootReference;
        Debug.Log(firebaseDatabase.RootReference);
        string key = reference.Child("PlayerInfo").Push().Key;
        reference.Child("PlayerInfo").Child(key).SetRawJsonValueAsync(json);
    }

    void GetPlayerInfo()
    {
        DatabaseReference reference = firebaseDatabase.GetReference("PlayerInfo");
        reference.GetValueAsync().ContinueWith(task => 
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var item in snapshot.Children)
                {
                    IDictionary playerInfoTemp = (IDictionary)item.Value;
                    playerInfo = new PlayerInfo
                    (
                        playerInfoTemp["nickname"].ToString(),
                        playerInfoTemp["uniqueID"].ToString(),
                        (int)playerInfoTemp["level"],
                        (int)playerInfoTemp["tiket"],
                        (int)playerInfoTemp["rank"],
                        (int)playerInfoTemp["gold"],
                        (int)playerInfoTemp["diamond"]
                    );
                    
                }
            }
        });
    }
}

public class PlayerInfo
{
    public string nickname;
    public string uniqueID;
    public Sprite profileImage;
    public int level;
    public int tiket;
    public int rank;
    public int gold;
    public int diamond;

    public PlayerInfo(string nickname, string uniqueID , int level, int badge, int rank, int gold, int diamond)
    {
        this.nickname = nickname;
        this.uniqueID = uniqueID;
        this.level = level;
        this.tiket = badge;
        this.rank = rank;
        this.gold = gold;
        this.diamond = diamond;
    }

    public override string ToString()
    {
        return $"nickname : {nickname} , level : {level} , tiket : {tiket} , rank : {rank} , gold : {gold} , diamond : {diamond}";
    }
}
