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
    [SerializeField] public int number;
}
public class Card : MonoBehaviourPun
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

   /* void OnMouseDown()
    {
        if (isActiveState)
        {
             Put();
        }
    }*/
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

        // ... 추후 CardManager.instance.RPC_ALL_Put(); 으로 변경
        //

        //gameManager.TurnEnd();
        isEfficient = false;
        Debug.Log(cardManager.OpenedCardDeck.Count);
        TurnManager.instance.RPC_ALL_EndTurn();

        isEfficient = false;
    }
    public void RPC_ALL_Put()
    {
        photonView.RPC(nameof(AddOpendCardDeck), RpcTarget.All);
    }
    [PunRPC]
    private void AddOpendCardDeck()
    {
        // 카드 매니저에 openedCardDeck 리스트에 추가
        cardManager.OpenedCardDeck.Add(this.gameObject);
        cardManager.transform.position = new Vector3(0, 3, 0);
        cardManager.OpenedCard = this.gameObject;
        cardManager.UpdateCardData();
    }
    /// <summary>
    /// 카드 이미지 셋팅
    /// </summary>
    protected virtual void Checking()
    {
        if (currentCardData.number == 7 || currentCardData.number == 8)
        {
            if (cardManager.CurrentCard.number == 14)
            {
                isActiveState = true;
            }
            else if (cardManager.CurrentCard.cardColor == currentCardData.cardColor || cardManager.CurrentCard.number == currentCardData.number)
            {
                isActiveState = true;
            }
            else
            {
                isActiveState = false;
            }
        }
        else if(currentCardData.number == 12)
        {
            if (cardManager.CurrentCard.number == 14)
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
        else if(currentCardData.number == 14)
        {
            isActiveState = true;
        }
        else if(currentCardData.number == 13)
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
            if (cardManager.CurrentCard.number == 14)
            {
                isActiveState = true;
            }
            else if (cardManager.CurrentCard.cardColor == currentCardData.cardColor || cardManager.CurrentCard.number == currentCardData.number)
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

        /*if (cardManager.CurrentCard.cardColor == cardData.cardColor || cardManager.CurrentCard.number == cardData.number)
        {
            isActiveState = true;
        }
        else if (cardData.number == 13)
        {
            if (cardManager.CurrentCard.cardColor == eCardColor.Black)
            {
                isActiveState = true;
            }
            else
            {
                isActiveState = false;
            }
        }
        else if (cardData.number == 14)
        {
            if (cardManager.CurrentCard.cardColor == eCardColor.Red)
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
        }*/
    }
}
