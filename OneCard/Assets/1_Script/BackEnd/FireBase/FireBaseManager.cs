using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.SceneManagement;

// FireBase 개체 생성
public class FireBaseManager : MonoBehaviour
{
    [SerializeField] public AuthManager authManager;
    [SerializeField] public DatabaseManager databaseManager;

    /// <summary>
    /// 현재 파이어베이스를 사용 가능한 환경인지 확인
    /// </summary>
    public bool IsFirebaseReady { get; private set; }
    public FirebaseApp FirebaseApp { get => firebaseApp; set => firebaseApp = value; }
    public AuthManager AuthManager { get => authManager; set => authManager = value; }
    public DatabaseManager DatabaseManager { get => databaseManager; set => databaseManager = value; }

    FirebaseApp firebaseApp;

    


    void Start()
    {
        StartCoroutine(CheckingLoding());
        InitializeFirebase();
    }

    
    void Update()
    {
       
    }
    IEnumerator CheckingLoding()
    {
        bool isAllDataLodingOver = false;
        while (isAllDataLodingOver == false)
        {
            isAllDataLodingOver = IsCheckingDataReady();
            if (isAllDataLodingOver == true)
            {
                Debug.Log("DataLodingFinish!");
                SceneManager.LoadScene("03_Main");
                break;
            }
            else if(isAllDataLodingOver == false)
            {
                Debug.Log($"DataLoding...{isAllDataLodingOver}");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public bool IsCheckingDataReady()
    {
        if(databaseManager.IsPlayerInfoDataReady == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var result = task.Result;

            if (result != DependencyStatus.Available) // Available: 파이어베이스 구동 가능 상태
            {
                // 파이어베이스 구동 가능 상태가 아니면

                Debug.LogError(result.ToString());
                IsFirebaseReady = false; // 구동불가상태 갱신 
            }
            else
            {
                IsFirebaseReady = true;

                firebaseApp = FirebaseApp.DefaultInstance;
                //firebaseAuth = FirebaseAuth.GetAuth(firebaseApp); // <= 똑같은 건데 자주 NULL 에러가 떠서 DefaultInstance로 쓰는게 좋음 

                authManager.InitAuthInstance();
                databaseManager.InitFirebaseDatabase(firebaseApp);

                authManager.AddEvtAuthStateChanged();
            }

            // signInButton.interactable = IsFirebaseReady;
        });
    }



}
