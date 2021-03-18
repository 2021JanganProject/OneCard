﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum eShape
{
    Heart,
    Diamond,
    Spade,
    Club,
    Joker
}

public enum eCardColor
{
    Black,
    Blue,
    Yellow,
    Red
}
public enum eCardType
{
    Nomarl,
    Ability,
    Special

}

[System.Serializable]
public struct CardData
{
    [SerializeField] public eShape shape;
    [SerializeField] public eCardColor cardColor;
    [SerializeField] public eCardType eCardType;
    [SerializeField] public int number;
}
public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] public CardData cardData;
    private CardManager cardManager;
    private GameManager gameManager;

    public bool isActiveState = false;

    public CardData CardData1 { get => cardData; set => cardData = value; }

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        gameManager = FindObjectOfType<GameManager>();
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
        cardManager.OpenedCardDeck.Add(this.gameObject);
        transform.position = new Vector3(0, 10, 0);
        cardManager.OpenedCard = this.gameObject;
        cardManager.UpdateCardData();
        gameManager.TurnEnd();
        Debug.Log(cardManager.OpenedCardDeck.Count);
    }
    /// <summary>
    /// 카드 이미지 셋팅
    /// </summary>
    public void SetCardImage(Sprite sprite)
    {
        if(cardImage == null)
        {
            cardImage = GetComponent<SpriteRenderer>();
        }
        cardImage.sprite = sprite;
    }

    protected virtual void Checking()
    {
        if (cardManager.CurrentCard.shape == cardData.shape || cardManager.CurrentCard.number == cardData.number)
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
        }
    }
}