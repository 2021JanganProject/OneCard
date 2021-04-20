using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;
using Photon.Pun;
using LitJson;

[System.Serializable]
public struct CardPos
{
    [SerializeField] public Transform CardStorage; // 다른 플레이어1 이 드로우한 카드를 담을 부모 게임오브젝트 
    [SerializeField] public Transform HandLeft;// 다른 플레이어1 의 카드패 보간을 위한 왼쪽 기준점
    [SerializeField] public Transform HandRight;// 다른 플레이어1 의 카드패 보간을 위한 오른쪽 기준점

    public CardPos(Transform cardStorage, Transform handLeft, Transform handRight)
    {
        CardStorage = cardStorage;
        HandLeft = handLeft;
        HandRight = handRight;
    }
}
public class CardManager : MonoBehaviourPun
{
    public static CardManager instance = null;

    public List<GameObject> MyCards { get => localCards; set => localCards = value; }


    public GameObject OpenedCard { get => openedCard; set => openedCard = value; }
    public CardData CurrentCard { get => currentCard; set => currentCard = value; }
    public List<GameObject> ClosedCardDeck { get => closedCardDeck; set => closedCardDeck = value; }
    public List<GameObject> OpenedCardDeck { get => openedCardDeck; set => openedCardDeck = value; }
    public int OpenedCardSortingOrder { get => openedCardSortingOrder; set => openedCardSortingOrder = value; }

    [SerializeField] private Sprite closedSprite;//원준
    [SerializeField] private SpriteAtlas cardAtlas;
    [SerializeField] private List<GameObject> closedCardDeck = new List<GameObject>();
    [SerializeField] private List<GameObject> openedCardDeck = new List<GameObject>();
    public CardPos[] RemoteCardPosArr { get => remoteCardPosArr; set => remoteCardPosArr = value; }

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardData currentCard;
    #region  == 카드패 Tranforms ==
    [Header("카드패 위치 (임시)")]
    [SerializeField] private Transform handLeft;// 카드패 확대 시 보간을 위한 왼쪽 기준점
    [SerializeField] private Transform handRight;// 카드패 확대 시 보간을 위한 오른쪽 기준점
    [SerializeField] private Transform mySecondHandLeft;// 카드패 확대 시 둘째줄 보간을 위한 왼쪽 기준점
    [SerializeField] private Transform mySecondHandRight;// 카드패 확대 시 둘째줄 보간을 위한 오른쪽 기준점
    [SerializeField] private Transform enlargeCardArea;// 카드를 꾹 눌러서 설명을 볼 때 카드가 이동할 위치

    [SerializeField] private Transform myCardStorage;// 내가 드로우한 카드를 담을 부모 게임오브젝트
    [SerializeField] private Transform myHandLeft;// 확대 전 내 카드패 보간을 위한 왼쪽 기준점
    [SerializeField] private Transform myHandRight;// 확대 전 내 카드패 보간을 위한 오른쪽 기준점

    [SerializeField] private CardPos[] remoteCardPosArr = new CardPos[3];

    #endregion
    [SerializeField] private List<GameObject> localCards = new List<GameObject>(); // 내가 뽑은 카드들을 담고있는 리스트

    [Header("Card 상위 개체")]
    [SerializeField] private GameObject openedCardBase;
    [SerializeField] private GameObject closedCardBase;
    private int maxCardNum = 13;
    private int maxCardColorNum = 5;
    private GameObject openedCard;
    private GameObject topClosedCard; // 뒤집혀져있는 카드중 제일 위에 있는 카드
    private UIManager uiManager;


    private string myCardTag = "MyCard"; // 내가 드로우한 카드의 태그를 바꿔주기 위한 변수
    private string myCardEnlargeTag = "MyCardEnlarge"; // 내 카드패를 확대했을 때 카드들의 태그를 바꿔주기 위한 변수
    private int openedCardSortingOrder = 1; // 낸 카드들의 소팅오더값을 증가시켜줄 변수

    private int turnIdxForTest = 0; // 턴에 따른 드로우 테스트 변수
    private int cardHandSortingOrderForTest = 1; // 카드패에 있는 카드들의 소팅오더 변수
    private int maxCardLineForTest = 6; // 내 카드패 확대 시 줄바꿈이 일어날 카드 갯수
    private bool isMineForTest = false; // 덱에서 뿌려진 카드가 내 카드인지 아닌지 판단할 변수(아니면 뒷면으로 나옴)
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
        myCardTag = "MyCard";
        myCardEnlargeTag = "MyCardEnlarge";
        openedCardSortingOrder = 1;
        cardHandSortingOrderForTest = 1;
        maxCardLineForTest = 6; //원준
        maxCardNum = 13; // 중간에 0으로 초기화 되는 버그가 있어서 강제로 다시 설정함.
        maxCardColorNum = 5;
        //SettingCard();
       
        StartCoroutine(DrawAtStart());
        StartCoroutine(AwaitInitlocalCards());

        //openedCard = closedCardDeck[0]; // @0415 버그로 임시 주석처리
        //UpdateCardData(); // @0415 버그로 임시 주석처리
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
        if (Input.GetKeyDown(KeyCode.Alpha1))// 카드 드로우 테스트
        {
            //if (turnIdxForTest == 0)
            //    isMineForTest = true;
            //else
            //    isMineForTest = false;
            //DrawCard(turnIdxForTest, isMineForTest);
            //turnIdxForTest++;
            //if (turnIdxForTest > 3)
            //{
            //    turnIdxForTest = 0;
            //}
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))// 카드 드로우 테스트 (시작 시)
        {
            StartCoroutine(DrawAtStart());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            localCards = GameManager.instance.LocalPlayerObj.GetComponent<Player>().MyCards; //@NeedRewok
        }
    }
    IEnumerator AwaitInitlocalCards()
    {
        while(true)
        {
            Debug.Log("AwaitInitMyCard...");
            if (GameManager.instance.LocalPlayerObj != null)
            {
                localCards = GameManager.instance.LocalPlayerObj.GetComponent<Player>().MyCards;
                Debug.Log("AwaitInitMyCard...");
                break;
            }
            yield return null;
        }
    }

    public void AddCloseCards()
    {
        GameObject[] cardsTemp = GameObject.FindGameObjectsWithTag("Card");
        for (int i = 0; i < cardsTemp.Length; i++)
        {
            closedCardDeck.Add(cardsTemp[i]);
        }
    }
    public void UpdateCardData() // 카드 정보 업데이트 7카드 때문
    {
        currentCard = openedCard.GetComponent<Card>().currentCardData;
    }
    public void RPC_ReQuest_DrawCard(int requestActorNum)
    {
        photonView.RPC(nameof(DrawCard), RpcTarget.MasterClient, requestActorNum);
        
        if (TurnManager.instance.IsMyturn())
        {
           
        }
    }
    // 마스터에서는 카드 값 동기화만 해주고
    // 나누거나 그런것들은 알아서 처리하게끔 하자
    [PunRPC]
    private void DrawCard(int requestActorNum) // 카드 드로우. 누구의 턴인가에 따라서 턴인 사람에게 카드가 추가됨 
    {
        var cardObj = closedCardDeck[0];
        //@원준: 마스터에서 뽑은 카드 => cardObj
        Card cardScript = cardObj.GetComponent<Card>();
        closedCardDeck.RemoveAt(0); // 마스터에서 지운다.

        photonView.RPC(nameof(SendToPlayer_DrawCardInfo), RpcTarget.AllViaServer, requestActorNum, cardScript.currentCardData.cardColor, cardScript.currentCardData.number);
    }

    [PunRPC]
    private void SendToPlayer_DrawCardInfo(int actorNum, int cardColorNum, int CardNum)
    {
        
        // 요청한 플레이어에게만 카드를 전달
        GameObject drawCardObj = closedCardDeck[0];

        for (int i = 0; i < GameManager.instance.PlayerArr.Length; i++)
        {
            int targetActorNumber = GameManager.instance.PlayerArr[i].PlayerActorIndex;
            Debug.Log(targetActorNumber);

            if (actorNum == targetActorNumber) // 요청 한 playerActorNum와 내가 가지고 있는 Player 목록과 일치했을 때만
            {
                // 카드 정보를 통해 다시 컴포넌트 생성
                InitCard(drawCardObj, cardColorNum, CardNum);
                closedCardDeck.RemoveAt(0);
            }
        }
        HandOutDrawCard(drawCardObj, actorNum);
    }
    private void HandOutDrawCard(GameObject cardObj, int requestActorNum)
    {
        DebugGUI.Warn($"1_requestActorNum_{requestActorNum}");
        if (TurnManager.instance.IsMyturn() == true)
        {
            //cardScript.UpdateCardState(eCardState.Opend);
            TurnManager.instance.CurrentTurnPlayer.MyCards.Add(cardObj); //
            cardObj.transform.parent = myCardStorage;
            cardObj.AddComponent<CardInteraction>();
            cardObj.tag = "MyCard";
            SetCardSortingOrderAndSortingLayerName(localCards, cardHandSortingOrderForTest);
            Debug.Log("내턴");
            AlignCard(requestActorNum);
        }
        else if (TurnManager.instance.IsMyturn() == false)
        {
            Player currentTurnPlayer = TurnManager.instance.CurrentTurnPlayer;
            GameObject [] remotePlayers = GameManager.instance.RemotePlayerObjArr;
            int remotePlayersIndex = -1;
            for (int i = 0; i < remotePlayers.Length; i++)
            {
                if(requestActorNum == remotePlayers[i].GetComponent<Player>().PlayerActorIndex)
                {
                    remotePlayersIndex = i;
                    DebugGUI.Warn($"2_remoteActorIndex_{remotePlayersIndex}");
                }
            }
            DebugGUI.Warn($"3_remoteActorIndex_{remotePlayersIndex}");
            //cardScript.UpdateCardState(eCardState.Closed);
            currentTurnPlayer.MyCards.Add(cardObj);
            cardObj.transform.parent = remoteCardPosArr[remotePlayersIndex].CardStorage; // @E
            SetCardSortingOrderAndSortingLayerName(TurnManager.instance.CurrentTurnPlayer.MyCards, cardHandSortingOrderForTest);
            Debug.Log("내턴 아님");
            AlignCard(remotePlayersIndex);
        }
       
    }
    public IEnumerator DrawAtStart()//처음 시작할 때 각 플레이어들이 5장씩 카드를 뽑는 함수(코루틴)
    {
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.2f);
            //if (turnIdxForTest == 0)
            //    isMineForTest = true;
            //else
            //    isMineForTest = false;
            //DrawCard(turnIdxForTest, isMineForTest);
            //turnIdxForTest++;
            //if (turnIdxForTest > 3)
            //    turnIdxForTest = 0;
        }

    }
   
    public void SettingCard()
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
                    
                    //GameObject cardTemp = Instantiate(cardPrefab, closedCardBase.transform); // OfflineCode
                    GameObject cardTemp = PhotonNetwork.Instantiate(cardPrefab.name, closedCardBase.transform.position , closedCardBase.transform.rotation);
                    InitCard(cardTemp, i, j);
                    // 리스트에 추가
                    closedCardDeck.Add(cardTemp);
                }
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    //GameObject cardTemp = Instantiate(cardPrefab, closedCardBase.transform); // OfflineCode
                    GameObject cardTemp = PhotonNetwork.Instantiate(cardPrefab.name, closedCardBase.transform.position, closedCardBase.transform.rotation);
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
                cardComponent.currentCardData.cardType = eCardType.ability;
                break;
            case 7:
            case 8:
                cardTemp.AddComponent<Attack>();
                cardComponent = cardTemp.GetComponent<Attack>();
                cardComponent.currentCardData.cardType = eCardType.ability;
                break;
            case 9:
                cardTemp.AddComponent<Jump>();
                cardComponent = cardTemp.GetComponent<Jump>();
                cardComponent.currentCardData.cardType = eCardType.ability;
                break;
            case 10:
                cardTemp.AddComponent<Back>();
                cardComponent = cardTemp.GetComponent<Back>();
                cardComponent.currentCardData.cardType = eCardType.ability;
                break;
            case 11:
                cardTemp.AddComponent<OneMore>();
                cardComponent = cardTemp.GetComponent<OneMore>();
                cardComponent.currentCardData.cardType = eCardType.ability;
                break;
            case 12:
                switch (cardColorNum)
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
                cardComponent.currentCardData.cardType = eCardType.Special;
                break;
            case 13:
                cardTemp.AddComponent<Attack>();
                cardComponent = cardTemp.GetComponent<Attack>();
                cardComponent.currentCardData.cardType = eCardType.Special;
                break;
            case 14:
                cardTemp.AddComponent<Smoke>();
                cardComponent = cardTemp.GetComponent<Smoke>();
                cardComponent.currentCardData.cardType = eCardType.Special;
                break;
            default:
                cardTemp.AddComponent<Card>();
                cardComponent = cardTemp.GetComponent<Card>();
                cardComponent.currentCardData.cardType = eCardType.Normal;
                break;
        }
        switch (cardColorNum)
        {
            case 0:
                cardComponent.currentCardData.cardColor = eCardColor.Black;
                break;
            case 1:
                cardComponent.currentCardData.cardColor = eCardColor.Blue;
                break;
            case 2:
                cardComponent.currentCardData.cardColor = eCardColor.Yellow;
                break;
            case 3:
                cardComponent.currentCardData.cardColor = eCardColor.Red;
                break;
            case 4:
                cardComponent.currentCardData.cardColor = eCardColor.Gray;
                break;
            default:
                return;
        }
        cardComponent.currentCardData.number = CardNum;
        cardTemp.transform.name = $"{cardComponent.currentCardData.cardColor}_{cardComponent.currentCardData.number + 1}";
        SetCardImage(cardComponent); // 카드 이미지 추가 로직
    }

    private void SetCardImage(Card card)
    {
        Sprite spriteTemp = GetAtlasSprite(card.currentCardData);
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
        string atlasName = $"{cardTypeString}_{num + 1}_of_{eCardColor.ToString().ToLower()}";
        return atlasName;
    }
    private Sprite GetAtlasSprite(CardData cardData)
    {
        // 이름 규칙 노션 논의 참고 노말카드 : n_num_of_color
        string atlasString = GetAtlasCardName(cardData.cardType, cardData.number, cardData.cardColor);
        return cardAtlas.GetSprite(atlasString);
    }
    #region  == CardChage Logic == 
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
    #endregion
    private void AddInitedCardsToColsedCardDeck()
    {

    }

    private void AllocateCard()
    {

    }

    private void AllocateCardAllPlayer()
    {

    }

    #region ==카드 이동 로직==

    public void ReduceMyCardHand() // 확대된 내 카드패를 다시 축소시킨 후 정렬
    {
        List<PosRot> cardPosAndRots = new List<PosRot>();

        List<GameObject> targetCards = new List<GameObject>();
        targetCards = localCards;

        cardPosAndRots = GetAlignCardsForCardPosRot(myHandLeft, myHandRight, targetCards.Count);
        SetAlignCardsToCardPosRots(targetCards, cardPosAndRots);
        for (int i = 0; i < localCards.Count; i++)
        {
            localCards[i].GetComponent<CardInteraction>().Invoke("SetFalseIsEnlargeCardHand", 0.3f);
            localCards[i].tag = myCardTag;
            localCards[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public void EnlargeMyCardHand() // 내 카드패를 확대시키고 갯수에 맞게 정렬
    {
        List<PosRot> cardPosAndRots = new List<PosRot>();
        List<PosRot> secondCardPpsAndRots = new List<PosRot>();
        List<GameObject> targetCards = new List<GameObject>();

        float zPos = 0f;
        float zPosForSecondCard = -0.06f;
        int count = localCards.Count;
        int secondCount = 6;
        if (count <= maxCardLineForTest)
        {
            cardPosAndRots = GetAlignMyCardRound(handLeft, handRight, localCards.Count, 0.5f, zPos);
            targetCards = localCards;
        }
        else
        {
            cardPosAndRots = GetAlignMyCardRound(handLeft, handRight, secondCount, 0.5f, zPos);
            secondCardPpsAndRots = GetAlignMyCardRound(mySecondHandLeft, mySecondHandRight, localCards.Count - maxCardLineForTest, 0.5f, zPosForSecondCard);
            Debug.Log("넘음");
            Debug.Log(localCards.Count);
            for (int i = 0; i < secondCount; i++)
            {
                targetCards.Add(localCards[i]);
            }
        }

        AlignMyCardsForLines(0, targetCards, cardPosAndRots);

        if (localCards.Count > maxCardLineForTest)
        {
            AlignMyCardsForLines(maxCardLineForTest, localCards, secondCardPpsAndRots);
        }
        for (int i = 0; i < localCards.Count; i++)
        {
            localCards[i].GetComponent<CardInteraction>().Invoke("SetTrueIsEnlargeCardHand", 0.3f);
            localCards[i].GetComponent<CardInteraction>().Invoke("SetOnColliderForInvoke", 0.3f);
            localCards[i].tag = myCardEnlargeTag;

        }
        Invoke("RememberCardPosAndRotSelfForInvoke", 0.3f);//DOTween함수가 끝난 후에 실행되도록 인보크로 호출
    }
    public void CallSetCardSortingOrderAndSortingLayerName()//카드의 소팅오더와 소팅레이어네임 변경
    {
        SetCardSortingOrderAndSortingLayerName(localCards, cardHandSortingOrderForTest);
    }
    //내 카드패 확대된 상태에서 각 카드를 꾹 누르면 해당 카드를 확대구역으로 이동시킴
    public void MoveCardToEnlargeArea(Transform cardTransform)
    {
        cardTransform.DOMove(enlargeCardArea.position, 0.5f);
        cardTransform.DORotateQuaternion(enlargeCardArea.rotation, 0.5f);

    }
    //확대구역에 있는 카드를 다시 원래의 위치로 이동시킴
    public void MoveCardToOrigin(Transform cardTransform, Vector3 originPos, Quaternion originRot)
    {
        cardTransform.DOMove(originPos, 0.3f);
        cardTransform.DORotateQuaternion(originRot, 0.3f);

    }
    //카드를 드래그,드롭 했을 때 OpenedCard와 충돌중이면 해당 카드를 OpenedCard 위치로 옮겨줌
    public void MoveCardToOpenedCardBase(Transform cardTransform)
    {
        cardTransform.DOMove(openedCardBase.transform.position, 0.3f);
        cardTransform.DORotateQuaternion(openedCardBase.transform.rotation, 0.3f);
        cardTransform.DOScale(openedCardBase.transform.localScale, 0.3f);
    }
    // 내 카드패가 확대됐을 때 줄에 따른 카드패 정렬 (첫째줄, 둘째줄 따로 정렬)
    private void AlignMyCardsForLines(int maxCardLine, List<GameObject> myCards, List<PosRot> PRs)
    {
        int j = 0;
        for (int i = maxCardLine; i < myCards.Count; i++)
        {
            var targetCard = myCards[i].GetComponent<Card>();
            var targetCardTransform = myCards[i].GetComponent<Transform>();
            targetCard.CardPosRot = PRs[j];
            MoveTransformForCard(targetCardTransform, targetCard.CardPosRot);

            j++;
        }

    }
    private void RememberCardPosAndRotSelfForInvoke() // 카드가 현재 있는 위치를 기억하게함. 카드를 드래그, 꾹 눌러 확대 했을 때 제자리로 돌아오기 위해
    {
        for (int i = 0; i < localCards.Count; i++)
        {

            localCards[i].GetComponent<CardInteraction>().SetPosAndRot();
        }
    }

    //카드패의 카드들의 소팅오더,소팅레이어네임을 변경해줌. 카드를 뽑을 때, 카드를 냈을 때 호출되어 조정
    private void SetCardSortingOrderAndSortingLayerName(List<GameObject> cards, int cardHandOrderForTest)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var sprite = cards[i].GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "CardOnHand";
            sprite.sortingOrder = cardHandOrderForTest++;
        }
    }
    //현재 턴인 사람(드로우를 한 사람)의 카드패를 새롭게 정렬해줌
    private void AlignCard(int turnIdx)
    {
        List<PosRot> cardPosRots = new List<PosRot>();
        List<GameObject> targetCards = new List<GameObject>(); // 자신의 카드패 
        DebugGUI.Error($"AlignCard_1_turnIdx_{turnIdx}");
        if (TurnManager.instance.IsMyturn() == true) 
        {
            targetCards = localCards;
            cardPosRots = GetAlignCardsForCardPosRot(myHandLeft, myHandRight, targetCards.Count);
            SetAlignCardsToCardPosRots(targetCards, cardPosRots);
        }
        else if (TurnManager.instance.IsMyturn() == false)
        {
            var remotePlayerObj = GameManager.instance.RemotePlayerObjArr[turnIdx];
            DebugGUI.Error($"remotePlayerObj_{remotePlayerObj.transform.name}");
            var remotePlayerScript = remotePlayerObj.GetComponent<Player>();
            
            targetCards = remotePlayerScript.MyCards;
            
            cardPosRots = GetAlignCardsForCardPosRot(remotePlayerScript.CardHandPos.HandLeft, remotePlayerScript.CardHandPos.HandRight, targetCards.Count);
            SetAlignCardsToCardPosRots(targetCards, cardPosRots);

            //foreach (GameObject children in playerScripts)
            //{
            //    var remotePlayerScript = children.GetComponent<Player>();
            //    if (remotePlayerScript == turnIdx)
            //    {
            //        targetCards = remotePlayerScript.MyCards;
            //        DebugGUI.Info(remotePlayerScript.CardHandPos.HandLeft);
            //        cardPosRots = GetAlignCardsForCardPosRot(remotePlayerScript.CardHandPos.HandLeft, remotePlayerScript.CardHandPos.HandRight, targetCards.Count);
            //        SetAlignCardsToCardPosRots(targetCards, cardPosRots);
            //    }
            //}

        }
    }
    //각 플레이어들의 카드패를 각각의 PRs 위치로 정렬시켜줌
    private void SetAlignCardsToCardPosRots(List<GameObject> myCards, List<PosRot> PRs)
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i].GetComponent<Card>();
            var targetCardTransform = targetCard.GetComponent<Transform>();
            targetCard.CardPosRot = PRs[i];
            MoveTransformForCard(targetCardTransform, targetCard.CardPosRot);
        }
    }
    //카드 드로우, 카드패 확대, 카드패 축소 시에 카드들의 위치를 DOTween으로 이동시켜줌
    private void MoveTransformForCard(Transform card, PosRot cardPR, float duration = 0.3f)
    {
        Debug.Log("카드 이동 호출");
        card.DOMove(cardPR.position, duration);
        card.DORotateQuaternion(cardPR.rotation, duration);
    }

    //플레이어가 보유한 카드갯수에 따라서 각 카드들의 위치를 카드패의 왼쪽,오른쪽 기준에 맞춰 보간하여 반환
    private List<PosRot> GetAlignCardsForCardPosRot(Transform leftTr, Transform rightTr, int cardCount)//원준 플레이어들 카드패 정렬
    {
        float[] cardLerps = new float[cardCount];
        List<PosRot> resultsCardPR = new List<PosRot>();
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
            resultsCardPR.Add(new PosRot(targetPos, targetRot));
        }
        return resultsCardPR;
    }
    //내 카드패의 카드들의 갯수에 따라 카드들의 위치,회전값을 카드패의 왼쪽,오른쪽 기준에 맞춰 선형보간,구면선형보간하여 반환함. 내 카드패 확대시에만 호출됨
    private List<PosRot> GetAlignMyCardRound(Transform leftTr, Transform rightTr, int cardCount, float height, float zPos)
    {
        float[] cardLerps = new float[cardCount];
        List<PosRot> resultsCardPR = new List<PosRot>();
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
            if (cardCount >= 6)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(cardLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, cardLerps[i]);
            }
            zPos += -0.01f;
            targetPos.z += zPos;
            resultsCardPR.Add(new PosRot(targetPos, targetRot));
        }
        return resultsCardPR;
    }

    #endregion

    private void ResetCard()
    {

    }
}