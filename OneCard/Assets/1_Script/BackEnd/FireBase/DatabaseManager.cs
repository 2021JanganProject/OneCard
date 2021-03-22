using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

// DB 생성
public class DatabaseManager : MonoBehaviour
{
    [SerializeField] FireBaseManager fireBaseManager;
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
        PlayerInfo playerInfo = new PlayerInfo("Guest", fireBaseManager.AuthManager.GetUserId(), 1, 5, 1, 100, 0);
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

    internal void InitFirebaseDatabase(FirebaseApp firebaseApp)
    {
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, DatabaseManager.FIREBASE_PATH);
    }

    void GetPlayerInfo(DataSnapshot item)
    {
        IDictionary playerInfoTemp = (IDictionary)item.Value;
        Debug.Log($"DB ID :{playerInfoTemp["uniqueID"].ToString()} , USER ID  : {fireBaseManager.AuthManager.GetUserId()} asd {playerInfoTemp["nickname"].ToString()}");
        if (playerInfoTemp["uniqueID"].ToString() == fireBaseManager.AuthManager.GetUserId())
        {
            Debug.Log("Find!");
            DataManager.instance.CurrentPlayerInfo = new PlayerInfo
            (
                playerInfoTemp["nickname"].ToString(),
                playerInfoTemp["uniqueID"].ToString(),
                int.Parse(playerInfoTemp["level"].ToString()),
                int.Parse(playerInfoTemp["tiket"].ToString()),
                int.Parse(playerInfoTemp["rank"].ToString()),
                int.Parse(playerInfoTemp["gold"].ToString()),
                int.Parse(playerInfoTemp["diamond"].ToString())
            );
            // = new PlayerInfo(PlayerInfo);
        }
    }

    public void GetAsyncGetPlayerInfo()
    {
        AsyncGetPlayerInfo();
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
                    Debug.Log($"DB ID :{playerInfoTemp["uniqueID"].ToString()} , USER ID  : {fireBaseManager.authManager.GetUserId()}");
                    if (playerInfoTemp["uniqueID"].ToString() == fireBaseManager.authManager.GetUserId())
                    {
                        Debug.Log("Find!");
                        //selectdPlayerInfoTemp = playerInfoTemp;
                        GetPlayerInfo(item);
                    }
                }
            }
        });
    }

    async void testtt()
    {
        DatabaseReference reference = firebaseDatabase.GetReference("PlayerInfo");
        reference.GetValueAsync().ContinueWith(task => { });
        


    }
}

