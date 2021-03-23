using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CardManager : MonoBehaviour
{
    public static CardManager instance = null;
    
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }
    public GameObject OpenedCard { get => openedCard; set => openedCard = value; }
    public CardData CurrentCard { get => currentCard; set => currentCard = value; }
    public List<GameObject> ClosedCardDeck { get => closedCardDeck; set => closedCardDeck = value; }
    public List<GameObject> OpenedCardDeck { get => openedCardDeck; set => openedCardDeck = value; }
    public CardManager(List<GameObject> openedCardDeck)
    {
        this.openedCardDeck = openedCardDeck;
    }

    [SerializeField] private SpriteAtlas cardAtlas;
    [SerializeField] private List<GameObject> closedCardDeck = new List<GameObject>();
    [SerializeField] private List<GameObject> openedCardDeck = new List<GameObject>();

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardData currentCard;

    [Header("Card 상위 개체")]
    [SerializeField] private GameObject openedCardBase;
    [SerializeField] private GameObject closedCardBase;
    private int maxCardNum = 13;
    private int maxCardColorNum = 5;
    [SerializeField] private int currentAttackCount = 0;
    private GameObject openedCard;
    private UIManager uiManager;



    #region == BtnEvts ==  
    public void BtnEvt_changeBlack()
    {
        ChangeBlack();
        QuitChangeCardColor();
    }
    public void BtnEvt_changeBlue()
    {
        ChangeBlue();
        QuitChangeCardColor();
    }
    public void BtnEvt_changeYellow()
    {
        ChangeYellow();
        QuitChangeCardColor();
    }
    public void BtnEvt_changeRed()
    {
        ChangeRed();
        QuitChangeCardColor();
    }
    #endregion

    void Start()
    {
        maxCardNum = 13; // 중간에 0으로 초기화 되는 버그가 있어서 강제로 다시 설정함.
        maxCardColorNum = 5;
        SettingCard();

        openedCard = closedCardDeck[0];
        UpdateCardData();
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (instance == null)
        {
            instance = this;
        }

    }

    private void Update()
    {

    }

    public void UpdateCardData()
    {
        // 카드 정보 업데이트 7카드 때문
        currentCard = openedCard.GetComponent<Card>().cardData;        
    }


    private void SettingCard()
    {
        InitCards();
    }

    private void InitCards()
    {
        for (int i = 0; i < maxCardColorNum; i++)
        {
            if (i < 4)
            {
                for (int j = 0; j < maxCardNum; j++)
                {
                    GameObject cardTemp = Instantiate(cardPrefab, closedCardBase.transform);
                    InitCard(cardTemp,i, j);
                    // 리스트에 추가
                    closedCardDeck.Add(cardTemp);
                }
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject cardTemp = Instantiate(cardPrefab , closedCardBase.transform);
                    InitCard(cardTemp, i, 13 + j);
                    // 리스트에 추가
                    closedCardDeck.Add(cardTemp);
                }
            }
        }
        ShuffleCards(closedCardDeck); //함수 하나당 하나의 작업 권장 - 박세찬 
    }

    private void InitCard(GameObject cardTemp, int cardColorNum, int CardNum)
    {
        Card cardComponent = null;
        switch (CardNum)
        {
            case 6:
                cardTemp.AddComponent<ChageShape>();
                cardComponent = cardTemp.GetComponent<ChageShape>();
                cardComponent.cardData.eCardType = eCardType.ability;
                break;
            case 7:
            case 8:
                cardTemp.AddComponent<Attack>();
                cardComponent = cardTemp.GetComponent<Attack>();
                cardComponent.cardData.eCardType = eCardType.ability;
                break;
            case 9:
                cardTemp.AddComponent<Jump>();
                cardComponent = cardTemp.GetComponent<Jump>();
                cardComponent.cardData.eCardType = eCardType.ability;
                break;
            case 10:
                cardTemp.AddComponent<Back>();
                cardComponent = cardTemp.GetComponent<Back>();
                cardComponent.cardData.eCardType = eCardType.ability;
                break;
            case 11:
                cardTemp.AddComponent<OneMore>();
                cardComponent = cardTemp.GetComponent<OneMore>();
                cardComponent.cardData.eCardType = eCardType.ability;
                break;
            case 12:
                switch(cardColorNum)
                {
                    case 0:
                        cardTemp.AddComponent<Attack>();
                        cardComponent = cardTemp.GetComponent<Attack>();
                        break;
                    case 1:
                        cardTemp.AddComponent<Defence>();
                        cardComponent = cardTemp.GetComponent<Defence>();
                        break;
                    case 2:
                        cardTemp.AddComponent<Card>();
                        cardComponent = cardTemp.GetComponent<Card>();
                        break;
                    case 3:
                        cardTemp.AddComponent<Card>();
                        cardComponent = cardTemp.GetComponent<Card>();
                        break;
                    default:
                        break;
                }
                cardComponent.cardData.eCardType = eCardType.Special;
                break;
            case 13:
                cardTemp.AddComponent<Attack>();
                cardComponent = cardTemp.GetComponent<Attack>();
                cardComponent.cardData.eCardType = eCardType.Special;
                break;
            case 14:
                cardTemp.AddComponent<Smoke>();
                cardComponent = cardTemp.GetComponent<Smoke>();
                cardComponent.cardData.eCardType = eCardType.Special;
                break;
            default:
                cardTemp.AddComponent<Card>();
                cardComponent = cardTemp.GetComponent<Card>();
                cardComponent.cardData.eCardType = eCardType.Normal;
                break;
        }
        switch (cardColorNum)
        {
            case 0:
                cardComponent.cardData.cardColor = eCardColor.Black;
                break;
            case 1:
                cardComponent.cardData.cardColor = eCardColor.Blue;
                break;
            case 2:
                cardComponent.cardData.cardColor = eCardColor.Yellow;
                break;
            case 3:
                cardComponent.cardData.cardColor = eCardColor.Red;
                break;
            case 4:
                cardComponent.cardData.cardColor = eCardColor.Gray;
                break;
            default:
                return;
        }
        cardComponent.cardData.number = CardNum;
        cardTemp.transform.name = $"{cardComponent.cardData.cardColor}_{cardComponent.cardData.number+1}";
        SetCardImage(cardComponent); // 카드 이미지 추가 로직
    }
    
    private void SetCardImage(Card card)
    {
        Sprite spriteTemp = GetAtlasSprite(card.cardData);
        card.SetCardImage(spriteTemp);
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

    private string GetAtlasCardName(eCardType eCardType, int num, eCardColor eCardColor)
    {
        string cardTypeString = null;
        switch (eCardType)
        {
            case eCardType.Normal:
                cardTypeString = "n";
                break;
            case eCardType.ability:
                cardTypeString = "ab";
                break;
            case eCardType.Special:
                cardTypeString = "spc";
                break;
            default:
                Debug.Assert(false, " ?? GetAtlasCardName Default");
                break;
        }
        string atlasName = $"{cardTypeString}_{num+1}_of_{eCardColor.ToString().ToLower()}";        
        return atlasName;
    }
    private Sprite GetAtlasSprite(CardData cardData)
    {
        // 이름 규칙 노션 논의 참고 노말카드 : n_num_of_color
        string atlasString = GetAtlasCardName(cardData.eCardType, cardData.number, cardData.cardColor);
        return cardAtlas.GetSprite(atlasString);
    }

    private void QuitChangeCardColor()
    {
        uiManager.ChangeShapeUI.SetActive(false);
    }

    private void ChangeBlack()
    {
        currentCard.cardColor = eCardColor.Black;
    }
    private void ChangeBlue()
    {
        currentCard.cardColor = eCardColor.Blue;
    }
    private void ChangeYellow()
    {
        currentCard.cardColor = eCardColor.Yellow;
    }
    private void ChangeRed()
    {
        currentCard.cardColor = eCardColor.Red;
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
