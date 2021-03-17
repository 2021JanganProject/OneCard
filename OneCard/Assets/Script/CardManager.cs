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

    [SerializeField] SpriteAtlas cardAtlas;
    [SerializeField] private List<GameObject> closedCardDeck = new List<GameObject>();
    [SerializeField] private List<GameObject> openedCardDeck = new List<GameObject>();

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardData currentCard;
    [Header("Card 상위 개체")]
    [SerializeField] GameObject opendCardBase;
    [SerializeField] GameObject closedCardBase;

    [SerializeField] private int currentAttackCount = 0;

    private GameObject openedCard;
    int maxCardNum = 13;
    int maxShapeNum = 5;

   

    private UIManager uiManager;


    #region == BtnEvts ==  
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
    #endregion

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
        maxCardNum = 13; // 중간에 0으로 초기화 되는 버그가 있어서 강제로 다시 설정함.
        maxShapeNum = 5;
        SettingCard();
        
        openedCard = closedCardDeck[0];
        UpdateCardData();
    }

    private void Update()
    {

    }
    private void SettingCard()
    {
        InitCards();
        //ShuffleCards(closedCardDeck);
    }
    private void InitCards()
    {
        for (int i = 0; i < maxShapeNum; i++)
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
        cardTemp.transform.name = $"{cardComponent.cardData.shape}_{cardComponent.cardData.number+1}";
        SetCardImage(cardComponent); // 카드 이미지 추가 로직
    }
    
    private void SetCardImage(Card card)
    {
        Sprite spriteTemp = GetAtlasSpriteForTest(card.cardData);
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


    string GetAtlasCardName(eCardType eCardType, int num, eCardColor eCardColor)
    {
        string cardTypeString = null;
        switch (eCardType)
        {
            case eCardType.Nomarl:
                cardTypeString = "n";
                break;
            case eCardType.Ability:
                cardTypeString = "ab";
                break;
            case eCardType.Special:
                cardTypeString = "spc";
                break;
            default:
                Debug.Assert(false, " ?? GetAtlasCardName Default");
                break;
        }
        string atlasName = $"{cardTypeString}_{num}_of_{eCardColor.ToString().ToLower()}";
        return atlasName;
    }
    private Sprite GetAtlasSpriteForTest(CardData cardData)
    {
        // 이름 규칙 노션 논의 참고 노말카드 : n_num_of_color
        string atlasString = GetAtlasCardName(cardData.eCardType, cardData.number, cardData.cardColor);
        return cardAtlas.GetSprite(atlasString);
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
