using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private List<object> closedCardDeck;
    private List<object> openedCardDeck;

    private Object opendCard;
    [SerializeField]
    private Object cardPrefab;
    private Card card;

    int maxCardNum = 12;
    int maxShapeNum = 3;

    void Start()
    {
        InitCards();
    }
    private void InitCards()
    {
        for (int i = 0; i < maxShapeNum; i++)
        {
            for (int j = 0; j < maxCardNum; j++)
            {
                //card.InitCard(i, j);
            }
        }
        //card.InitCard(4,13);
        //card.InitCard(4,14);

    }

    private void ShuffleCards(List<GameObject> Card)
    {
        
    }

    private void AddInitedCardsToColsedCardDeck()
    {

    }

    private void AllocateCard()
    {

    }

    private void AllocateCardAllPlayer()
    {

    }

    public void DrawCard()
    {

    }

    private void ResetCard()
    {

    }
}
