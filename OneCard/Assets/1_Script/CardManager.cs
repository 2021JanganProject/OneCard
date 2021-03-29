using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;

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
    [SerializeField] private Sprite closedSprite;//원준
    [SerializeField] private SpriteAtlas cardAtlas;
    [SerializeField] private List<GameObject> closedCardDeck = new List<GameObject>();
    [SerializeField] private List<GameObject> openedCardDeck = new List<GameObject>();

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardData currentCard;

    [Header("카드패 위치 (임시)")]
    [SerializeField] private Transform handLeft;//원준
    [SerializeField] private Transform handRight;//원준
    [SerializeField] private Transform mySecondHandLeft;//원준
    [SerializeField] private Transform mySecondHandRight;//원준

    [SerializeField] private Transform myCardStorage; //원준
    [SerializeField] private Transform myHandLeft;//원준
    [SerializeField] private Transform myHandRight;//원준

    [SerializeField] private Transform firstCardStorage; //원준
    [SerializeField] private Transform firstHandLeft;//원준
    [SerializeField] private Transform firstHandRight;//원준

    [SerializeField] private Transform secondCardStorage; //원준
    [SerializeField] private Transform secondHandLeft;//원준
    [SerializeField] private Transform secondHandRight;//원준

    [SerializeField] private Transform lastCardStorage; //원준
    [SerializeField] private Transform lastHandLeft;//원준
    [SerializeField] private Transform lastHandRight;//원준
    
    [SerializeField] private List<GameObject> myCards = new List<GameObject>(); //원준
    [SerializeField] private List<GameObject> firstotherCards = new List<GameObject>();//원준
    [SerializeField] private List<GameObject> secondotherCards = new List<GameObject>();//원준
    [SerializeField] private List<GameObject> lastotherCards = new List<GameObject>();//원준

    [Header("Card 상위 개체")]
    [SerializeField] private GameObject openedCardBase;
    [SerializeField] private GameObject closedCardBase;
    private int maxCardNum = 13;
    private int maxCardColorNum = 5;
    [SerializeField] private int currentAttackCount = 0;
    private GameObject openedCard;
    private UIManager uiManager;

    private int turnIdxForTest = 0;
    private int cardHandOrderForTest = 1; //원준
    private int maxCardLineForTest = 6; //원준
    private bool isMineForTest = false; //원준
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
        cardHandOrderForTest = 1;
        maxCardLineForTest = 6; //원준
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
        if (Input.GetKeyDown(KeyCode.Keypad1))//원준
        {
            if (turnIdxForTest == 0)
                isMineForTest = true;
            else
                isMineForTest = false;
            DrawCard(turnIdxForTest, isMineForTest);
            turnIdxForTest++;
            if (turnIdxForTest > 3)
                turnIdxForTest = 0;
            
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))//원준
        {
                StartCoroutine(DrawAtStart());    
        }   
    }
    public void DrawCard(int turnIdx, bool isMine)
    {
        var card = closedCardDeck[0];//원준
        switch (turnIdx)
        {
            case 0:
                myCards.Add(card);
                card.transform.parent = myCardStorage;
                card.AddComponent<PlayCard>();
                
                break;
            case 1:
                firstotherCards.Add(card);
                card.transform.parent = firstCardStorage;
                break;
            case 2:
                secondotherCards.Add(card);
                card.transform.parent = secondCardStorage;
                break;
            case 3:
                lastotherCards.Add(card);
                card.transform.parent = lastCardStorage;
                break;
            default:
                Debug.Log("에러");
                break;
        }
        closedCardDeck.RemoveAt(0);
        if (!isMine)
            card.GetComponent<Card>().SetCardImage(closedSprite);

        SetOrderLayer(card);
        AlignCard(turnIdx);
    }
    public IEnumerator DrawAtStart()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.2f);
            if (turnIdxForTest == 0)
                isMineForTest = true;
            else
                isMineForTest = false;
            DrawCard(turnIdxForTest, isMineForTest);
            turnIdxForTest++;
            if (turnIdxForTest > 3)
                turnIdxForTest = 0;
        }

    }
    public void UpdateCardData()
    {
        // 카드 정보 업데이트 7카드 때문
        currentCard = openedCard.GetComponent<Card>().cardData;        
    }
    public void ReduceMyCardHand()
    {
        List<PR> cardPRs = new List<PR>();

        List<GameObject> targetCards = new List<GameObject>();
        targetCards = myCards;

        cardPRs = AlignCards(myHandLeft, myHandRight, targetCards.Count);
        AlignOtherCardsToPR(targetCards, cardPRs);
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].GetComponent<PlayCard>().SetEnlarge(false);
        }
    }
    public void EnlargeMyCardHand()
    {
        List<PR> cardPRs = new List<PR>();
        List<PR> secondCardPRs = new List<PR>();
        List<GameObject> targetCards = new List<GameObject>();

        int count = myCards.Count;
        int secondCount = 6;
        if (count <= maxCardLineForTest)
        {
            cardPRs = AlignMyCardRound(handLeft, handRight, myCards.Count, 0.5f);
            targetCards = myCards;
        }
        else
        {
            cardPRs = AlignMyCardRound(handLeft, handRight, secondCount, 0.5f);
            secondCardPRs = AlignMyCardRound(mySecondHandLeft, mySecondHandRight, myCards.Count - maxCardLineForTest, 0.5f);
            Debug.Log("넘음");
            Debug.Log(myCards.Count);
            for (int i = 0; i < secondCount; i++)
            {
                targetCards.Add(myCards[i]);
            }
        }

        AlignMyCardForLines(0, targetCards, cardPRs);

        if (myCards.Count > maxCardLineForTest)
        {
            AlignMyCardForLines(maxCardLineForTest, myCards, secondCardPRs);
        }
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].GetComponent<PlayCard>().SetEnlarge(true);
        }
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
    private Sprite GetAtlasSpriteForTest(CardData cardData)
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
    private void SetOrderLayer(GameObject card)//원준
    {
        var sprite = card.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = cardHandOrderForTest;
        cardHandOrderForTest++;
    }
    //내 카드패 갯수에 따른 카드패 정렬, 줄바꿈
    private void AlignCard(int turnIdxForTest)
    {
        List<PR> cardPRs = new List<PR>();

        List<GameObject> targetCards = new List<GameObject>();
        switch (turnIdxForTest)
        {
            case 0:
                targetCards = myCards;
                cardPRs = AlignCards(myHandLeft, myHandRight, targetCards.Count);
                AlignOtherCardsToPR(targetCards, cardPRs);
                break;
            case 1:
                targetCards = firstotherCards;
                cardPRs = AlignCards(firstHandLeft, firstHandRight, targetCards.Count);
                AlignOtherCardsToPR(targetCards, cardPRs);
                break;
            case 2:
                targetCards = secondotherCards;
                cardPRs = AlignCards(secondHandLeft, secondHandRight, targetCards.Count);
                AlignOtherCardsToPR(targetCards, cardPRs);
                break;
            case 3:
                targetCards = lastotherCards;
                cardPRs = AlignCards(lastHandLeft, lastHandRight, targetCards.Count);
                AlignOtherCardsToPR(targetCards, cardPRs);
                break;
            default:
                Debug.Log("에러"); break;
        } 
    }

    private void AlignMyCardForLines(int maxCardLine, List<GameObject> myCards, List<PR> PRs)//원준
    {
        int j = 0;
        for (int i = maxCardLine; i < myCards.Count; i++)
        {
            var targetCard = myCards[i].GetComponent<Card>();
            var targetCardTransform = myCards[i].GetComponent<Transform>();
            targetCard.CardPR = PRs[j];
            MoveTransformForDrewCard(targetCardTransform, targetCard.CardPR);
            j++;
        }
        
    }
    private void AlignOtherCardsToPR(List<GameObject> myCards, List<PR> PRs)//원준
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i].GetComponent<Card>();
            var targetCardTransform = targetCard.GetComponent<Transform>();
            targetCard.CardPR = PRs[i];
            MoveTransformForDrewCard(targetCardTransform, targetCard.CardPR);
        }
    }
    private void MoveTransformForDrewCard(Transform card, PR cardPR, float duration = 0.3f)//원준 카드패 정렬 시 패의 모든 카드들 위치 변경
    {
        card.DOMove(cardPR.position, duration);
        card.DORotateQuaternion(cardPR.rotation, duration);

    }
    private List<PR> AlignCards(Transform leftTr, Transform rightTr, int cardCount)//원준 플레이어들 카드패 정렬
    {
        float[] cardLerps = new float[cardCount];
        List<PR> resultsCardPR = new List<PR>();
        switch (cardCount)
        {
            case 1: cardLerps = new float[] { 0.5f }; break;
            case 2: cardLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: cardLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (cardCount - 1);
                for (int i = 0; i < cardCount; i++)
                    cardLerps[i] = interval * i;
                break;
        }
        for (int i = 0; i < cardCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, cardLerps[i]);
            var targetRot = Quaternion.identity;
            resultsCardPR.Add(new PR(targetPos, targetRot));
        }
        return resultsCardPR;
    }
    //내 카드패 정렬
    private List<PR> AlignMyCardRound(Transform leftTr, Transform rightTr, int cardCount, float height)
    {
        float[] cardLerps = new float[cardCount];
        List<PR> resultsCardPR = new List<PR>();
        switch (cardCount)
        {
            case 1: cardLerps = new float[] { 0.5f }; break;
            case 2: cardLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: cardLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (cardCount - 1);
                for (int i = 0; i < cardCount; i++)
                    cardLerps[i] = interval * i;
                break;
        }
        for (int i = 0; i < cardCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, cardLerps[i]);
            var targetRot = Quaternion.identity;
            if(cardCount >= 6)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(cardLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, cardLerps[i]);
            }
            resultsCardPR.Add(new PR(targetPos, targetRot));
        }
        return resultsCardPR;
    }
    private void ResetCard()
    {

    }
}
