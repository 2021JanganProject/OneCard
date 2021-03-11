using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager instance;
  
    [SerializeField] public FireBaseManager fireBaseManager;
    [SerializeField] public Login login;

    public delegate void lodingOverEvent();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // 시작 시 데이터 갱신되면 시작
        OnWaitLodingOver(fireBaseManager.authManager.IsLogined() , login.LoginLoadingOver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnWaitLodingOver(bool trigger , lodingOverEvent onLodingOver)
    {
        StartCoroutine(WaitForUserInstanceLodingOver(trigger , onLodingOver));
    }
    // User 인스턴스 생성까지 대기
    // 대기 이후 로그인 상태 확인
    // 로그인 되어 있으면 바로 넘어가기 
    // 로그인 안되어 있으면 로그인 창 보여주고 로그인 유도
    IEnumerator WaitForUserInstanceLodingOver(bool trigger, lodingOverEvent onLodingOver)
    {
        while (fireBaseManager.authManager.IsLoginHasInstance() == false) // 비동기 로그인 대기
        {
            Debug.Log($"비동기 로그인 대기중{fireBaseManager.authManager.IsLoginHasInstance()}");
            yield return null;
        }
        Debug.Log($"로그인 상황 {fireBaseManager.authManager.IsLogined()}");
        //finish
        if (fireBaseManager.authManager.IsLogined())
        {
           
            fireBaseManager.databaseManager.GetAsyncGetPlayerInfo();
            login.Signedin();
        }
        else
        {
            login.SignedOut();
        }
    }
}
