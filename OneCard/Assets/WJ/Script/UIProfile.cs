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
    
    //private int PlayerCount;
    [SerializeField] Player player;
    [SerializeField] private GameObject myTurnImageForTest;
    [SerializeField] private GameObject nextTurnImageForTest;
    [SerializeField] private GameObject waitImageForTest;

    private ePlayerState playerstate;


    private void ChangeTurnImageForPlayerState()
    {
        if (myTurnImageForTest != null && nextTurnImageForTest != null && waitImageForTest != null)
        {
            if (playerstate == ePlayerState.myTurn)
            {
                waitImageForTest.SetActive(false);
                nextTurnImageForTest.SetActive(false);
                myTurnImageForTest.SetActive(true);
            }
            else if (playerstate == ePlayerState.NextTurn)
            {
                waitImageForTest.SetActive(false);
                myTurnImageForTest.SetActive(false);
                nextTurnImageForTest.SetActive(true);
            }
            else
            {
                myTurnImageForTest.SetActive(false);
                nextTurnImageForTest.SetActive(false);
                waitImageForTest.SetActive(true);
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
        playerNickNameText.text = player.PlayerNickname;
        playerRankText.text = player.PlayerRank;
        playerImageSprite.sprite = player.PlayerImage.sprite;
    }

    
    // Update is called once per frame
    private void Update()
    {
        playerstate = player.PlayerState;
        ChangeTurnImageForPlayerState();
    }
}
