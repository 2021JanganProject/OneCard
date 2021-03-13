using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager instance = null;
    [SerializeField]
    private List<GameObject> closedCardDeck;
    public List<GameObject> openedCardDeck;

    public GameObject openedCard;
    [SerializeField]
    public GameObject cardPrefab;

    int maxCardNum = 13;
    int maxShapeNum = 5;
    public int currentAttackCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        InitCards();
        openedCard = closedCardDeck[0];
    }

    private void InitCards()
    {
        for (int i = 0; i < maxShapeNum; i++)
        {
            if (i < 4)
            {
                for (int j = 0; j < maxCardNum; j++)
                {
                    GameObject firstCard = Instantiate(cardPrefab);
                    Card card = firstCard.GetComponent<Card>();
                    card.InitCard(i, j);
                    // 리스트에 추가
                    closedCardDeck.Add(firstCard);
                }
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject firstCard = Instantiate(cardPrefab);
                    Card card = firstCard.GetComponent<Card>();
                    card.InitCard(i, 13 + j);
                    // 리스트에 추가
                    closedCardDeck.Add(firstCard);
                }
            }
        }
        ShuffleCards(closedCardDeck);
    }

    private void ShuffleCards(List<GameObject> Card)
    {
        int rnd1;
        int rnd2;

        for (int i = 0; i < Card.Count; i++)
        {
            rnd1 = Random.Range(0, Card.Count);
            rnd2 = Random.Range(0, Card.Count);

            var tmp = Card[rnd1];
            Card[rnd1] = Card[rnd2];
            Card[rnd2] = tmp;
        }
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
