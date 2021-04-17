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
    private AttackCounter attackCounter;
    [SerializeField] private List<GameObject> cardDeck;
    [SerializeField] private List<Card> cardDataDeck;
    [SerializeField] private eAiState eAiState;
    // Start is called before the first frame update
    void Start()
    {
        card = FindObjectOfType<Card>();
        cardManager = FindObjectOfType<CardManager>();
        attackCounter = FindObjectOfType<AttackCounter>();
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
        cardDataDeck.Add(cardManager.ClosedCardDeck[0].GetComponent<Card>());
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
            int cardNum = -1;
            int cardRank = 0;
            for (int i = 0; i < cardDeck.Count; i++)
            {
                if (cardDataDeck[i].isActiveState)
                {
                    /// 카드에 우선순위를 만들어 주고 싶은데 어캐 해야할지 모르겠음(대충 if문으로 하면 되는거 알았음)
                    /// 해당 카드가 되면 for문에서 벗어날수 있는 방안이 필요(답은 break였음, for문에서도 break가 적용된다......)
                    /// 이러면 찾는게 불가해짐 서치가 가능해야한다.                
                    if (attackCounter.CurrentAttackCount == 0)
                    {
                        // 공격받지 않은경우

                        if (cardDataDeck[i].currentCardData.cardNumberIndex == 11)
                        {
                            cardNum = i;
                            cardRank = 8;
                        }
                        else if (cardRank < 8 && cardDataDeck[i].currentCardData.cardNumberIndex == 14)
                        {
                            cardNum = i;
                            cardRank = 7;
                        }
                        else if (cardRank < 7 && (cardDataDeck[i].currentCardData.cardNumberIndex == 6 || cardDataDeck[i].currentCardData.cardNumberIndex == 9 || cardDataDeck[i].currentCardData.cardNumberIndex == 10))
                        {
                            cardNum = i;
                            cardRank = 6;
                        }
                        else if (cardRank < 6 && cardDataDeck[i].currentCardData.cardNumberIndex > 0 && cardDataDeck[i].currentCardData.cardNumberIndex < 5)
                        {
                            cardNum = i;
                            cardRank = 5;
                        }
                        else if (cardRank < 5 && (cardDataDeck[i].currentCardData.cardNumberIndex == 13 || (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor != eCardColor.Blue)))
                        {
                            cardNum = i;
                            cardRank = 4;
                        }
                        else if (cardRank < 4 && cardDataDeck[i].currentCardData.cardNumberIndex == 8)
                        {
                            cardNum = i;
                            cardRank = 3;
                        }
                        else if (cardRank < 3 && cardDataDeck[i].currentCardData.cardNumberIndex == 7)
                        {
                            cardNum = i;
                            cardRank = 2;
                        }
                        else if (cardRank < 2 && cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Blue)
                        {
                            cardNum = i;
                            cardRank = 1;
                        }

                        /* AI 초기 방안 (가장 큰 숫자를 찾아서 내는 로직)
                         * if (cardDataDeck[i].CardData1.number >= cardType)
                        {
                            cardType = cardDataDeck[i].CardData1.number;
                            cardNum = i;
                        }
                        Debug.Log(cardType);
                         */
                        Debug.Log(cardRank);
                    }
                    else if(attackCounter.CurrentAttackCount > 0)
                    {
                        // 공격을 받은 경우
                        if(cardDataDeck[i].currentCardData.cardNumberIndex == 7)
                        {
                            cardNum = i;
                            cardRank = 5;
                        }
                        else if(cardRank < 5 && cardDataDeck[i].currentCardData.cardNumberIndex == 8)
                        {
                            cardNum = i;
                            cardRank = 4;
                        }
                        else if (cardRank < 4 && (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Black))
                        {
                            cardNum = i;
                            cardRank = 3;
                        }
                        else if (cardRank < 3 && (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Blue))
                        {
                            cardNum = i;
                            cardRank = 2;
                        }
                        else if (cardRank < 2 && cardDataDeck[i].currentCardData.cardNumberIndex == 14)
                        {
                            cardNum = i;
                            cardRank = 1;
                        }
                    }
                }
            }
            Debug.Log(cardNum);
            if (cardNum == -1)
            {
                DrawTest();
            }
            else
            {
                cardDeck[cardNum].GetComponent<Card>().IsEfficient = true;
                cardDeck.RemoveAt(cardNum);
                cardDataDeck.RemoveAt(cardNum);
            }
        }
    }

    private void AiAttackerPut()
    {
        // 가지고 있는 카드 중 가장 합리적인 카드를 내는 함수
        if (cardDeck.Count < 1)
        {
            // 갖고 있는 카드가 한장도 없을시 드로우
            DrawTest();
        }
        else
        {
            int cardNum = -1;
            int cardRank = 0;
            for (int i = 0; i < cardDeck.Count; i++)
            {
                if (cardDataDeck[i].isActiveState)
                {            
                    if (attackCounter.CurrentAttackCount == 0)
                    {
                        if (cardDataDeck[i].currentCardData.cardNumberIndex == 11)
                        {
                            cardNum = i;
                            cardRank = 9;
                        }
                        else if (cardRank < 9 && (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Black))
                        {
                            cardNum = i;
                            cardRank = 8;
                        }
                        else if (cardRank < 8 && cardDataDeck[i].currentCardData.cardNumberIndex == 8)
                        {
                            cardNum = i;
                            cardRank = 7;
                        }
                        else if (cardRank < 7 && cardDataDeck[i].currentCardData.cardNumberIndex == 7)
                        {
                            cardNum = i;
                            cardRank = 6;
                        }
                        else if (cardRank < 6 && (cardDataDeck[i].currentCardData.cardNumberIndex == 6 || cardDataDeck[i].currentCardData.cardNumberIndex == 9 || cardDataDeck[i].currentCardData.cardNumberIndex == 10))
                        {
                            cardNum = i;
                            cardRank = 5;
                        }
                        else if (cardRank < 5 && cardDataDeck[i].currentCardData.cardNumberIndex > 0 && cardDataDeck[i].currentCardData.cardNumberIndex < 5)
                        {
                            cardNum = i;
                            cardRank = 4;
                        }
                        else if (cardRank < 4 && (cardDataDeck[i].currentCardData.cardNumberIndex == 13 || (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor != eCardColor.Blue && cardDataDeck[i].currentCardData.cardColor != eCardColor.Black)))
                        {
                            cardNum = i;
                            cardRank = 3;
                        }
                        else if (cardRank < 3 && cardDataDeck[i].currentCardData.cardNumberIndex == 14)
                        {
                            cardNum = i;
                            cardRank = 2;
                        }
                        else if (cardRank < 2 && cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Blue)
                        {
                            cardNum = i;
                            cardRank = 1;
                        }
                        Debug.Log(cardRank);
                    }
                    else if (attackCounter.CurrentAttackCount > 0)
                    {
                        if (cardDataDeck[i].currentCardData.cardNumberIndex == 7)
                        {
                            cardNum = i;
                            cardRank = 5;
                        }
                        else if (cardRank < 5 && cardDataDeck[i].currentCardData.cardNumberIndex == 8)
                        {
                            cardNum = i;
                            cardRank = 4;
                        }
                        else if (cardRank < 4 && (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Black))
                        {
                            cardNum = i;
                            cardRank = 3;
                        }
                        else if (cardRank < 3 && (cardDataDeck[i].currentCardData.cardNumberIndex == 12 && cardDataDeck[i].currentCardData.cardColor == eCardColor.Blue))
                        {
                            cardNum = i;
                            cardRank = 2;
                        }
                        else if (cardRank < 2 && cardDataDeck[i].currentCardData.cardNumberIndex == 14)
                        {
                            cardNum = i;
                            cardRank = 1;
                        }
                    }
                }
            }
            Debug.Log(cardNum);
            if (cardNum == -1)
            {
                DrawTest();
            }
            else
            {
                cardDeck[cardNum].GetComponent<Card>().IsEfficient = true;
                cardDeck.RemoveAt(cardNum);
                cardDataDeck.RemoveAt(cardNum);
            }
        }
    }

    private void AiDefencerPut()
    {

    }
}
