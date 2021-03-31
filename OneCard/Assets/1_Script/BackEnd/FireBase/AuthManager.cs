
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
// 로그인
public class AuthManager : MonoBehaviour
{
    public FirebaseUser User { get => user; set => user = value; }
    public FirebaseAuth FirebaseAuth { get => firebaseAuth; set => firebaseAuth = value; }
    [SerializeField] FireBaseManager fireBaseManager;
    FirebaseAuth firebaseAuth; // auth 용 instance
    FirebaseUser user; // 사용자

    /// <summary>
    /// 중복 클릭이 되지 않게끔 확인
    /// </summary>
    public bool IsSignInOnProgress { get; private set; }


    string displayName;
    string emailAddress;
    string photoUrl;

    void Start()
    {
        // 초기화
        
        DebugerManager.instance.del_debugInputT += () =>
        {
            //DebugerManager.instance.IsNull(firebaseAuth);
            //DebugerManager.instance.IsNull(firebaseApp);
            //DebugerManager.instance.Log(GetUserId());
            //DebugerManager.instance.IsNull(DatabaseManager.instance.reference);
        };
    }

    public bool IsLoginHasInstance()
    {
        if (User == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsLogined()
    {
        
        if(user == null)
        {
            return false;
        }
        else
        {
            Debug.Log($"user :: {user.UserId}");
            return true;
        }
    }

    public string GetUserId()
    {
        return user.UserId;
    }
   
    public void InitAuthInstance()
    {
        firebaseAuth = FirebaseAuth.DefaultInstance;
    }

    public void AddEvtAuthStateChanged()
    {
        firebaseAuth.StateChanged += AuthStateChanged;
    }
    /** 상태변화 추적 */
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (firebaseAuth.CurrentUser != user)
        {
            bool signedIn = user != firebaseAuth.CurrentUser && firebaseAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.LogFormat("Signed out {0}", user.UserId);
            }
            user = firebaseAuth.CurrentUser;
            if (signedIn)
            {
                DebugerManager.instance.Log(string.Format("Signed in {0}", user.UserId));
                displayName = user.DisplayName ?? "";
                emailAddress = user.Email ?? "";
                DebugerManager.instance.Log(string.Format("Signed in displayName {0} _ emailAddress {1}", displayName, emailAddress));
            }
        }
    }

    public void BtnEvt_anoymousLogin()
    {
        anoymousLogin();
    }
    public void BtnEvt_anoymousLogout()
    {
        anoymousLogout();
    }

    private void anoymousLogout()
    {
        DebugerManager.instance.Log(string.Format("User Logout"));
        firebaseAuth.SignOut();
        
    }
    /** 익명 로그인 요청 */
    private void anoymousLogin()
    {
        StartCoroutine(WaitForLoginCompleted());
        firebaseAuth.SignInAnonymouslyAsync().ContinueWith(task => {
              if (task.IsCanceled)
              {
                  Debug.LogError("SignInAnonymouslyAsync was canceled.");
                  return;
              }
              if (task.IsFaulted)
              {
                  Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                  return;
              }
              if(task.IsCompleted)
              {
                  user = task.Result;
                  DebugerManager.instance.Log(string.Format("User signed in successfully: DisplayName {0} UserId ({1})",
                  user.DisplayName, user.UserId));
                  
              }
          });
    }

    IEnumerator WaitForLoginCompleted()
    {
        Debug.Log("Start");
        while (user == null)
        {
            yield return null;
        }
        Debug.Log("Start InitFirstLoginUserDatabase");
        //IsUserReady = true;
        fireBaseManager.DatabaseManager.InitFirstLoginUserDatabase();
    }

}
