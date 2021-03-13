using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*3.11 UIProfile 기능
 * 플레이어가 가지고있는 정보를 얻어와서 UI에 띄움, 플레이어 턴 상태에 따라 마이턴,다음턴,기다리는 중 이미지 표시 */
public class UIProfile : MonoBehaviour
{
    [SerializeField]
    private Text playerNickNameText;
    [SerializeField]
    private Text playerRankText;
    [SerializeField]
    private Image playerImageSprite;
    [SerializeField]
    private string PlayerNickName;
    [SerializeField]
    private string PlayerRank;
    [SerializeField]
    private Image PlayerImage;

    //private int PlayerCount;

    [SerializeField]
    Player player;
    [SerializeField]
    private GameObject myTurnImage;
    [SerializeField]
    private GameObject nextTurnImage;
    [SerializeField]
    private GameObject WaitImage;

    private PlayerState playerstate;

    private PlayerState myTurn = PlayerState.myTurn;
    private PlayerState NextTurn = PlayerState.NextTurn;
    private PlayerState Wait = PlayerState.Wait;

    

    private void Awake()
    {
        
        playerstate = player.getPlayerState();       
        //PlayerCount = turnManager.PlayerCount;
        
    }
    // Start is called before the first frame update
    void Start()
    {

            PlayerNickName = player.getPlayerNickname();
            PlayerRank = player.getPlayerRank();
            PlayerImage = player.getPlayerImage();

            playerNickNameText.text = PlayerNickName;
            playerRankText.text = PlayerRank;
            playerImageSprite.sprite = PlayerImage.sprite;
      
        
    }
    
    // Update is called once per frame
    void Update()
    {
        playerstate = player.getPlayerState();
        ChangeTurnImageForPlayerState();


    }
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
}
