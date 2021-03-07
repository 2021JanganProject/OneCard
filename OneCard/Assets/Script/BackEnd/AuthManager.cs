
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class AuthManager : MonoBehaviour
{
    /// <summary>
    /// 현재 파이어베이스를 사용 가능한 환경인지 확인
    /// </summary>
    public bool IsFirebaseReady { get; private set; }
    /// <summary>
    /// 중복 클릭이 되지 않게끔 확인
    /// </summary>
    public bool IsSignInOnProgress { get; private set; }

    /** auth 용 instance */
    FirebaseAuth firebaseAuth;
    /** 사용자 */
    FirebaseUser user;
    FirebaseApp firebaseApp;
    private FirebaseDatabase firebaseDatabase;

    string displayName;
    string emailAddress;
    string photoUrl;

    private void Awake()
    {
        
    }
    void Start()
    {
        // 초기화
        InitializeFirebase();
        Debuger.instance.debugDel += () =>
        {
            Debuger.instance.IsNull(firebaseAuth);
            Debuger.instance.IsNull(firebaseDatabase);
        };
    }

    bool IsLogined()
    {
        if(user == null)
        {
            return false;
        }
        else
        {
            return true;
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
                firebaseAuth = FirebaseAuth.DefaultInstance;
                firebaseDatabase = FirebaseDatabase.DefaultInstance;
                firebaseAuth.StateChanged += AuthStateChanged;
            }

            // signInButton.interactable = IsFirebaseReady;
        });

        
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
                Debuger.instance.Log(string.Format("Signed in {0}", user.UserId));
                displayName = user.DisplayName ?? "";
                emailAddress = user.Email ?? "";
                Debuger.instance.Log(string.Format("Signed in displayName {0} _ emailAddress {1}", displayName, emailAddress));
            }
        }
        List<object> t = new List<object>();
        t.Sort();
        // cardList.Sort();
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
        firebaseAuth.SignOut();
        Debuger.instance.Log(string.Format("User signed in successfully: {0} ({1})",
            user.DisplayName, user.UserId));
    }
    /** 익명 로그인 요청 */
    private void anoymousLogin()
    {
        firebaseAuth
          .SignInAnonymouslyAsync()
          .ContinueWith(task => {
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
              user = task.Result;
              Debuger.instance.Log(string.Format("User signed in successfully: DisplayName {0} UserId ({1})",
              user.DisplayName, user.UserId));
          });
    }
}
