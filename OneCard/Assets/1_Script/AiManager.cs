using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAiState
{
    Normal,
    Attacker,
    Defencer
}
public class AiManager : MonoBehaviour
{
    private Card card;
    private CardManager cardManager;
    [SerializeField] private List<GameObject> cardDeck;
    [SerializeField] private eAiState eAiState;
    // Start is called before the first frame update
    void Start()
    {
        card = FindObjectOfType<Card>();
        cardManager = FindObjectOfType<CardManager>();
        
    }

    #region == BtnEvts ==  
    public void BtnEvt_DrawTest()
    {
        DrawTest();
    }
    public void BtnEvt_PutTest()
    {
        switch(eAiState)
        {
            case eAiState.Normal:
                AiNormalPut();
                break;
            case eAiState.Attacker:
                AiAttackerPut();
                break;
            case eAiState.Defencer:
                AiDefencerPut();
                break;
            default:
                break;
        }
    }
    #endregion

    private void DrawTest()
    {
        // ai가 카드를 받아오는 함수
        cardDeck.Add(cardManager.ClosedCardDeck[0]);
        cardManager.ClosedCardDeck.RemoveAt(0);
    }

    private void AiNormalPut()
    {
        // 가지고 있는 카드 중 가장 합리적인 카드를 내는 함수
        if (cardDeck.Count < 1)
        {
            // 갖고 있는 카드가 한장도 없을시 드로우
            DrawTest();
        }
        else
        {
            int cardType = -1;
            int cardNum = -1;
            for (int i = 0; i < cardDeck.Count; i++)
            {
                if (cardDeck[i].GetComponent<Card>().isActiveState)
                {
                    // 카드에 우선순위를 만들어 주고 싶은데 어캐 해야할지 모르겠음
                    if (cardDeck[i].GetComponent<Card>().CardData1.number >= cardType)
                    {
                        cardType = cardDeck[i].GetComponent<Card>().CardData1.number;
                        cardNum = i;
                    }
                    Debug.Log(cardType);
                }
            }
            if (cardNum == -1)
            {
                DrawTest();
            }
            else
            {
                cardDeck[cardNum].GetComponent<Card>().IsEfficient = true;
                cardDeck.RemoveAt(cardNum);
            }
        }
    }

    private void AiAttackerPut()
    {

    }

    private void AiDefencerPut()
    {

    }
}
