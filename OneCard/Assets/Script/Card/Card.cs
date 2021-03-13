using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Shape
{
    Heart,
    Diamond,
    Spade,
    Club,
    Joker
}

public enum CardColor
{
    Black,
    Red
}

[System.Serializable]
public struct CardData
{
    [SerializeField] public Shape shape;
    [SerializeField] public CardColor color;
    [SerializeField] public int number;
}
public class Card : MonoBehaviour
{



    private Image cardImage;
    [SerializeField] CardData cardData;
    private CardManager cardManager;

    public int currnetNum;
    public string currentShape;
    public bool isActiveState = false;

    public CardData CardData1 { get => cardData; set => cardData = value; }

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
    }
    void Update()
    {
        Checking();
    }

    void OnMouseDown()
    {
        if (isActiveState)
        {
            Put();
        }
    }

    protected virtual void Put()
    {
        // 카드 매니저에 openedCardDeck 리스트에 추가
        cardManager.openedCardDeck.Add(this.gameObject);
        transform.position = new Vector3(0, 10, 0);
        cardManager.openedCard = this.gameObject;
        Debug.Log(cardManager.openedCardDeck.Count);
    }



    protected virtual void Checking()
    {
        // 같은 문양인지 OR 같은 숫자인지 확인
        /// 왜 NULL값이 반환되는지 모르겠음
        /// attack에서 실행할때만 오류
        var compareCard = cardManager.openedCard.GetComponent<Card>();
        if (compareCard.currentShape == this.currentShape || compareCard.currnetNum == this.currnetNum)
        {
            isActiveState = true;
        }
        else if (currnetNum == 13)
        {
            if (cardData.color == CardColor.Black)
            {
                isActiveState = true;
            }
            else
            {
                isActiveState = false;
            }
        }
        else if (currnetNum == 14)
        {
            if (cardData.color == CardColor.Red)
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
        Debug.Log("체크");
    }

    public void InitCard(int ShapeNum, int CardNum)
    {
        switch (ShapeNum)
        {
            case 0:
                cardData.shape = Shape.Heart;
                cardData.color = CardColor.Red;
                break;
            case 1:
                cardData.shape = Shape.Diamond;
                cardData.color = CardColor.Red;
                break;
            case 2:
                cardData.shape = Shape.Spade;
                cardData.color = CardColor.Black;
                break;
            case 3:
                cardData.shape = Shape.Club;
                cardData.color = CardColor.Black;
                break;
            case 4:
                cardData.shape = Shape.Joker;
                break;
            default:
                return;
        }
        switch (CardNum)
        {
            case 0:
            case 1:
                this.gameObject.AddComponent<Attack>();
                break;
            case 2:
                this.gameObject.AddComponent<Defence>();
                break;
            case 6:
                this.gameObject.AddComponent<ChageShape>();
                break;
            case 10:
                this.gameObject.AddComponent<Jump>();
                break;
            case 11:
                this.gameObject.AddComponent<Back>();
                break;
            case 12:
                this.gameObject.AddComponent<OneMore>();
                break;
            default:
                this.gameObject.AddComponent<Card>();
                break;

        }
        cardData.number = CardNum;
        // 조커 색상
        if (CardNum == 13)
        {
            cardData.color = CardColor.Black;
            this.gameObject.AddComponent<Attack>();
        }
        else if (CardNum == 14)
        {
            cardData.color = CardColor.Red;
            this.gameObject.AddComponent<Attack>();
        }
        currnetNum = cardData.number;
        currentShape = cardData.shape.ToString();
    }
}
