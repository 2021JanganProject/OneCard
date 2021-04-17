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
        // ai�� ī�带 �޾ƿ��� �Լ�
        cardDeck.Add(cardManager.ClosedCardDeck[0]);
        cardDataDeck.Add(cardManager.ClosedCardDeck[0].GetComponent<Card>());
        cardManager.ClosedCardDeck.RemoveAt(0);
    }

    private void AiNormalPut()
    {
        // ������ �ִ� ī�� �� ���� �ո����� ī�带 ���� �Լ�
        if (cardDeck.Count < 1)
        {
            // ���� �ִ� ī�尡 ���嵵 ������ ��ο�
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
                    /// ī�忡 �켱������ ����� �ְ� ������ ��ĳ �ؾ����� �𸣰���(���� if������ �ϸ� �Ǵ°� �˾���)
                    /// �ش� ī�尡 �Ǹ� for������ ����� �ִ� ����� �ʿ�(���� break����, for�������� break�� ����ȴ�......)
                    /// �̷��� ã�°� �Ұ����� ��ġ�� �����ؾ��Ѵ�.                
                    if (attackCounter.CurrentAttackCount == 0)
                    {
                        // ���ݹ��� �������

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

                        /* AI �ʱ� ��� (���� ū ���ڸ� ã�Ƽ� ���� ����)
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
                        // ������ ���� ���
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
        // ������ �ִ� ī�� �� ���� �ո����� ī�带 ���� �Լ�
        if (cardDeck.Count < 1)
        {
            // ���� �ִ� ī�尡 ���嵵 ������ ��ο�
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
