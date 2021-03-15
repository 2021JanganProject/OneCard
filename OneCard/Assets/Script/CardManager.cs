using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager instance = null;
    [SerializeField] private List<GameObject> closedCardDeck;
    public List<GameObject> openedCardDeck;

    private GameObject openedCard;

    [SerializeField] public GameObject cardPrefab;
    [SerializeField] private CardData currentCard;

    int maxCardNum = 13;
    int maxShapeNum = 5;
    [SerializeField] private int currentAttackCount = 0;
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public GameObject OpenedCard { get => openedCard; set => openedCard = value; }
    public CardData CurrentCard { get => currentCard; set => currentCard = value; }

    private UIManager uiManager;

    public void BtnEvt_changeHeart()
    {
        ChangeHeart();
        QuitChangeShape();
    }
    public void BtnEvt_changeDiamond()
    {
        ChangeDiamond();
        QuitChangeShape();
    }
    public void BtnEvt_changeSpade()
    {
        ChangeSpade();
        QuitChangeShape();
    }
    public void BtnEvt_changeClub()
    {
        ChangeClub();
        QuitChangeShape();
    }

    public void UpdateCardData()
    {
        // 카드 정보 업데이트 7카드 때문
        currentCard = openedCard.GetComponent<Card>().cardData;        
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        InitCards();
        openedCard = closedCardDeck[0];
        UpdateCardData();
    }

    private void Update()
    {

    }

    private void InitCards()
    {
        for (int i = 0; i < maxShapeNum; i++)
        {
            if (i < 4)
            {
                for (int j = 0; j < maxCardNum; j++)
                {
                    GameObject cardTemp = Instantiate(cardPrefab);
                    InitCard(cardTemp,i, j);
                    // 리스트에 추가
                    closedCardDeck.Add(cardTemp);
                }
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject cardTemp = Instantiate(cardPrefab);
                    InitCard(cardTemp, i, 13 + j);
                    // 리스트에 추가
                    closedCardDeck.Add(cardTemp);
                }
            }
        }
        ShuffleCards(closedCardDeck);
    }

    private void InitCard(GameObject cardTemp, int ShapeNum, int CardNum)
    {
        Card cardComponent = null;
        switch (CardNum)
        {
            case 0:
            case 1:
                cardTemp.AddComponent<Attack>();
                cardComponent = cardTemp.GetComponent<Attack>();
                break;
            case 2:
                cardTemp.AddComponent<Defence>();
                cardComponent = cardTemp.GetComponent<Defence>();
                break;
            case 6:
                cardTemp.AddComponent<ChageShape>();
                cardComponent = cardTemp.GetComponent<ChageShape>();
                break;
            case 10:
                cardTemp.AddComponent<Jump>();
                cardComponent = cardTemp.GetComponent<Jump>();
                break;
            case 11:
                cardTemp.AddComponent<Back>();
                cardComponent = cardTemp.GetComponent<Back>();
                break;
            case 12:
                cardTemp.AddComponent<OneMore>();
                cardComponent = cardTemp.GetComponent<OneMore>();
                break;
            case 13:
            case 14:
                cardTemp.AddComponent<Attack>();
                cardComponent = cardTemp.GetComponent<Attack>();
                break;
            default:
                cardTemp.AddComponent<Card>();
                cardComponent = cardTemp.GetComponent<Card>();
                break;
        }
        if (CardNum == 13)
        {
            cardComponent.cardData.cardColor = eCardColor.Black;            
        }
        else if (CardNum == 14)
        {
            cardComponent.cardData.cardColor = eCardColor.Red;
        }
        switch (ShapeNum)
        {
            case 0:
                cardComponent.cardData.shape = eShape.Heart;
                cardComponent.cardData.cardColor = eCardColor.Red;
                break;
            case 1:
                cardComponent.cardData.shape = eShape.Diamond;
                cardComponent.cardData.cardColor = eCardColor.Red;
                break;
            case 2:
                cardComponent.cardData.shape = eShape.Spade;
                cardComponent.cardData.cardColor = eCardColor.Black;
                break;
            case 3:
                cardComponent.cardData.shape = eShape.Club;
                cardComponent.cardData.cardColor = eCardColor.Black;
                break;
            case 4:
                cardComponent.cardData.shape = eShape.Joker;
                break;
            default:
                return;
        }
        cardComponent.cardData.number = CardNum;
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

    private void QuitChangeShape()
    {
        uiManager.ChangeShapeUI.SetActive(false);
    }

    private void ChangeHeart()
    {
        currentCard.shape = eShape.Heart;
        currentCard.cardColor = eCardColor.Red;
    }
    private void ChangeDiamond()
    {
        currentCard.shape = eShape.Diamond;
        currentCard.cardColor = eCardColor.Red;
    }
    private void ChangeSpade()
    {
        currentCard.shape = eShape.Spade;
        currentCard.cardColor = eCardColor.Black;
    }
    private void ChangeClub()
    {
        currentCard.shape = eShape.Club;
        currentCard.cardColor = eCardColor.Black;
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
