using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum eCardColor
{
    Black,
    Blue,
    Yellow,
    Red,
    Gray
}
public enum eCardType
{
    Normal,
    ability,
    Special
}
public enum eCardState
{
    Opend,
    Closed
}


[System.Serializable]
public struct CardData
{
    [SerializeField] public eCardColor cardColor;
    [SerializeField] public eCardType cardType;
    [SerializeField] public eCardState cardState;
    [SerializeField] public int cardNumberIndex;
}
public class Card : MonoBehaviour
{
    public PosRot CardPosRot { get => cardPosRot; set => cardPosRot = value; }
    private PosRot cardPosRot;//원준
    /// <summary>
    /// 
    /// </summary>
    public bool IsMine { get => isMine; set => isMine = value; }

    private PosRot cardPR;//원준

    public bool isActiveState = false;
    [SerializeField] public CardData currentCardData;
    [SerializeField] private SpriteRenderer cardImage; 
    [SerializeField] private Sprite cardSpriteTemp;
    [SerializeField] private Sprite closedSprite;

    private CardManager cardManager;
    private GameManager gameManager;
    private SpriteRenderer renderer;
    [SerializeField] private bool isEfficient = false;
    private AttackCounter attackCounter;
    //public CardData currentCardData { get => cardData; set => cardData = value; }
    public bool IsEfficient { get => isEfficient; set => isEfficient = value; }

    private bool isMine;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        gameManager = FindObjectOfType<GameManager>();
        renderer = GetComponent<SpriteRenderer>();
        attackCounter = FindObjectOfType<AttackCounter>();
    }
    void Update()
    {
        Checking();
        if(isActiveState)
        {
            renderer.color = new Color(1, 1, 1, 1);
        }
        else
        {
            renderer.color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
        if(isEfficient)
        {
            Put();
        }
    }

    public void UpdateCardState(eCardState cardState)
    {
        currentCardData.cardState = cardState;
        CardOpendClosedImageUpdate();

    }

    private void CardOpendClosedImageUpdate()
    {
        switch(currentCardData.cardState)
        {
            case eCardState.Closed:
                cardImage.sprite = closedSprite;
                break;
            case eCardState.Opend:
                cardImage.sprite = cardSpriteTemp;
                break;
            default:
                Debug.Log("CardOpendClosedImageUpdate Error");
                break;
        }
    }

    void OnMouseDown()
    {
        if (isActiveState)
        {
            Put();
        }        
    }
    public void SetCardImage(Sprite sprite)
    {
        if (cardImage == null)
        {
            cardImage = GetComponent<SpriteRenderer>();
        }
        cardImage.sprite = sprite;
        cardSpriteTemp = sprite;
    }

    protected virtual void Put()
    {
        // 카드 매니저에 openedCardDeck 리스트에 추가
        cardManager.OpenedCardDeck.Add(this.gameObject);
        transform.position = new Vector3(0, 3, 0);
        cardManager.OpenedCard = this.gameObject;
        cardManager.UpdateCardData();
        //gameManager.TurnEnd();
        isEfficient = false;
        Debug.Log(cardManager.OpenedCardDeck.Count);
    }
    /// <summary>
    /// 카드 이미지 셋팅
    /// </summary>
    protected virtual void Checking()
    {
        if (currentCardData.cardNumberIndex == 7 || currentCardData.cardNumberIndex == 8)
        {
            if (cardManager.CurrentCard.cardNumberIndex == 14)
            {
                isActiveState = true;
            }
            else if (cardManager.CurrentCard.cardColor == currentCardData.cardColor || cardManager.CurrentCard.cardNumberIndex == currentCardData.cardNumberIndex)
            {
                isActiveState = true;
            }
            else
            {
                isActiveState = false;
            }
        }
        else if(currentCardData.cardNumberIndex == 12)
        {
            if (cardManager.CurrentCard.cardNumberIndex == 14)
            {
                isActiveState = true;
            }
            else if (cardManager.CurrentCard.cardColor == currentCardData.cardColor)
            {
                if (currentCardData.cardColor == eCardColor.Red || currentCardData.cardColor == eCardColor.Yellow)
                {
                    if (attackCounter.CurrentAttackCount < 1)
                    {
                        isActiveState = true;
                    }
                    else
                    {
                        isActiveState = false;
                    }
                }
            }
            else
            {
                isActiveState = false;
            }
        }
        else if(currentCardData.cardNumberIndex == 14)
        {
            isActiveState = true;
        }
        else if(currentCardData.cardNumberIndex == 13)
        {
            if(attackCounter.CurrentAttackCount == 0)
            {
                isActiveState = true;
            }
            else
            {
                isActiveState = false;
            }
        }
        else
        {
            if (cardManager.CurrentCard.cardNumberIndex == 14)
            {
                isActiveState = true;
            }
            else if (cardManager.CurrentCard.cardColor == currentCardData.cardColor || cardManager.CurrentCard.cardNumberIndex == currentCardData.cardNumberIndex)
            {
                if(attackCounter.CurrentAttackCount < 1)
                {
                    isActiveState = true;
                }
                else
                {
                    isActiveState = false;
                }
            }            
            else
            {
                isActiveState = false;
            }
        }
    }
}
