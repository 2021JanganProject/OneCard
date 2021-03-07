using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
public class DatabaseManager : MonoBehaviour
{
    const string FIREBASE_PATH = "https://fir-demo-project.firebaseio.com/";
    public DatabaseReference reference { get; set; }
    private FirebaseDatabase firebaseDatabase;

    // Start is called before the first frame update
    void Start()
    {
        //InitFirebaseDatabase();
        Debuger.instance.debugDel += InitFirebaseDatabase;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitFirebaseDatabase()
    {
        firebaseDatabase = FirebaseDatabase.DefaultInstance;
    }

    void SetData()
    {
        PlayerInfo playerInfo = new PlayerInfo("Test", 1, 5, 4, 4, 4);
        string json = JsonUtility.ToJson(playerInfo);

        firebaseDatabase.GetReference(FIREBASE_PATH).SetRawJsonValueAsync(json);

        //reference.Child()
    }

}

class PlayerInfo
{
    public string nickname;
    public int level;
    public int badge;
    public int rank;
    public int gold;
    public int diamond;

    public PlayerInfo(string nickname, int level, int badge, int rank, int gold, int diamond)
    {
        this.nickname = nickname;
        this.level = level;
        this.badge = badge;
        this.rank = rank;
        this.gold = gold;
        this.diamond = diamond;
    }
}
