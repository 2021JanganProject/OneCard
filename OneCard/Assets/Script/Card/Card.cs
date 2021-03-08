using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    protected enum Shape
    {
        Heart,
        Diamond,
        Spade,
        Club,
        Joker
    }

    private enum Color
    {
        Black,
        Red
    }

    private struct CardData
    {
        public Shape shape;
        public Color color;
        public int number;
    }

    private Image cardImage;
    CardData cardData;
    private CardManager cardManager;

    public int currnetNum;
    public string currentShape;

    void Start()
    {
        cardManager = GetComponent<CardManager>();
    }

    protected virtual void Put()
    {

    }

    protected virtual void Checking()
    {
        // 같은 문양인지 확인
        // CardManager.openedCard.cardData.Shape == cardData.Shape;
    }

    public void InitCard(int ShapeNum, int CardNum)
    {
        switch(ShapeNum)
        {
            case 0 :
                cardData.shape = Shape.Heart;
                cardData.color = Color.Red;
                break;
            case 1:
                cardData.shape = Shape.Diamond;
                cardData.color = Color.Red;
                break;
            case 2:
                cardData.shape = Shape.Spade;
                cardData.color = Color.Black;
                break;
            case 3:
                cardData.shape = Shape.Club;
                cardData.color = Color.Black;
                break;
            case 4:
                cardData.shape = Shape.Joker;
                break;
            default :
                return;
        }
        switch(CardNum)
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
        }
                cardData.number = CardNum;
        // 조커 색상
        if(CardNum == 13)
        {
            cardData.color = Color.Black;
            this.gameObject.AddComponent<Attack>();
        }
        else if(CardNum == 14)
        {
            cardData.color = Color.Red;
            this.gameObject.AddComponent<Attack>();
        }
        currnetNum = cardData.number;
        currentShape = cardData.shape.ToString();
    }
}
