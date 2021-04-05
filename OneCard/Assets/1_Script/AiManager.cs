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
        // ai�� ī�带 �޾ƿ��� �Լ�
        cardDeck.Add(cardManager.ClosedCardDeck[0]);
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
            int cardType = -1;
            int cardNum = -1;
            for (int i = 0; i < cardDeck.Count; i++)
            {
                if (cardDeck[i].GetComponent<Card>().isActiveState)
                {
                    // ī�忡 �켱������ ����� �ְ� ������ ��ĳ �ؾ����� �𸣰���
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
