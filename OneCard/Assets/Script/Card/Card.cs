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
        Club
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

    protected virtual void Put()
    {

    }

    protected virtual void Checking()
    {

    }

    public void InitCard()
    {

    }
}
