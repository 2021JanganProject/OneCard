using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private enum Shape
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

    public int currnetNum;
    public string currentShape;

    protected virtual void Put()
    {

    }

    protected virtual void Checking()
    {

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
        cardData.number = CardNum;

        // 조커 색상
        if(CardNum == 13)
        {
            cardData.color = Color.Black;
        }
        else if(CardNum == 14)
        {
            cardData.color = Color.Red;
        }
        currnetNum = cardData.number;
        currentShape = cardData.shape.ToString();
    }
}
