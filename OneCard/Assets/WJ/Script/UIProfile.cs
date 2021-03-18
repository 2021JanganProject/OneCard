using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*3.11 UIProfile 기능
 * 플레이어가 가지고있는 정보를 얻어와서 UI에 띄움, 플레이어 턴 상태에 따라 마이턴,다음턴,기다리는 중 이미지 표시 */
public class UIProfile : MonoBehaviour
{
    [SerializeField] private Text playerNickNameText;
    [SerializeField] private Text playerRankText;
    [SerializeField] private Image playerImageSprite;
    [SerializeField] private string playerNickName;
    [SerializeField] private string playerRank;
    [SerializeField] private Image playerImage;
    //private int PlayerCount;
    [SerializeField] Player player;
    [SerializeField] private GameObject myTurnImage;
    [SerializeField] private GameObject nextTurnImage;
    [SerializeField] private GameObject WaitImage;
    private ePlayerState playerstate;
    private ePlayerState myTurn = ePlayerState.myTurn;
    private ePlayerState NextTurn = ePlayerState.NextTurn;
    private ePlayerState Wait = ePlayerState.Wait;


    private void ChangeTurnImageForPlayerState()
    {
        if (myTurnImage != null && nextTurnImage != null && WaitImage != null)
        {
            if (playerstate == myTurn)
            {
                WaitImage.SetActive(false);
                nextTurnImage.SetActive(false);
                myTurnImage.SetActive(true);
            }
            else if (playerstate == NextTurn)
            {
                WaitImage.SetActive(false);
                myTurnImage.SetActive(false);
                nextTurnImage.SetActive(true);
            }
            else
            {
                myTurnImage.SetActive(false);
                nextTurnImage.SetActive(false);
                WaitImage.SetActive(true);
            }
        }
    }
    private void Awake()
    {
        playerstate = player.PlayerState;       
    }
    // Start is called before the first frame update
    private void Start()
    {
        SetUIProfile();
    }

    private void SetUIProfile()
    {
        playerNickName = player.PlayerNickname;
        playerRank = player.PlayerRank;
        playerImage = player.PlayerImage;

        playerNickNameText.text = playerNickName;
        playerRankText.text = playerRank;
        playerImageSprite.sprite = playerImage.sprite;
    }

    
    // Update is called once per frame
    private void Update()
    {
        playerstate = player.PlayerState;
        ChangeTurnImageForPlayerState();
    }
}
