using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1"; // 게임 버전이 다르면 같이 매칭이 안되게 해야하기 떄문이다. 
    [SerializeField] private Button joinButton;
    public bool IsHojunForDebug= false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion; // 게임 버전을 명시 
        PhotonNetwork.ConnectUsingSettings(); // 설정 정보(게임버전 등)를 가지고 마스터 서버에 접속  

        joinButton.interactable = false;
        Debug.Log("마스터 서버에 접속 중");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster() // 마스터 서버 접속 성공 했을 때
    {
        joinButton.interactable = true;
        Debug.Log("Online!");
    }

    public override void OnDisconnected(DisconnectCause cause) // 실패한 경우
    {
        joinButton.interactable = false;
        Debug.Log($"Offline! {cause.ToString()}");
        PhotonNetwork.ConnectUsingSettings(); // 설정 정보(게임버전 등)를 가지고 마스터 서버에 접속  // 재접속의 의미이기도 함 

    }

    public void BtnEvt_Connect() // BtnEvnt 접속
    {
        joinButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("랜덤한 방으로 접속 시도");
            PhotonNetwork.JoinRandomRoom(); // 랜덤 룸 접속
        }
        else
        {
            Debug.Log($"Offline! ToString()");
            joinButton.interactable = false;
            PhotonNetwork.ConnectUsingSettings(); // 설정 정보(게임버전 등)를 가지고 마스터 서버에 재접속  
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // JoinRandomRoom 중 실패 했을 때
    {
        // 빈방이 없을 때
        Debug.Log("빈방이 없습니다. 새로운 방을 만듭니다");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 },null);
    }

    public override void OnJoinedRoom()// JoinRandomRoom 참가 완료 
    {
        Debug.Log("접속 완료!");
        // 일반적인 씬매니저로 씬 이동을 하면 해당 사용자만 넘어간다.
        
        if(IsHojunForDebug) // 호준 테스트 코드
        {
            PhotonNetwork.LoadLevel("Game_Test");
            return;
        }
        PhotonNetwork.LoadLevel("04_Game"); // player 모두 같은 씬으로 넘어갈 수 있게끔 자동으로 동기화 
    }

 
}
