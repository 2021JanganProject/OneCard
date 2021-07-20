using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TouchManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform myCardHand;

    private bool isEnlargeCard = false;
    private bool isEnlargeCardHand = false;
    private CardManager CM;
    private TurnManager TM;
    private GameObject currentCard;
    float time = 0f;

    private void Awake()
    {
        myCardHand = GameObject.Find("MyCardHand").GetComponent<Transform>();
    }

    void Start()
    {
        CM = CardManager.instance;
        TM = TurnManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!isEnlargeCard)
                    {
                        int layerMask = (-1) - (1 << LayerMask.NameToLayer("InvisibleWall"));
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);

                        if (hit.collider != null && hit.collider.transform == myCardHand)
                        {
                            isEnlargeCardHand = true;
                            Debug.Log(isEnlargeCardHand);
                            ModulateCardHandSize(isEnlargeCardHand);

                        }
                        else if (hit.collider != null && isEnlargeCardHand == true && hit.collider.gameObject.tag == "MyCardEnlarge")
                        {
                            currentCard = hit.transform.gameObject;
                        }
                        else
                        {
                            isEnlargeCardHand = false;
                            Debug.Log(isEnlargeCardHand);
                            ModulateCardHandSize(isEnlargeCardHand);
                            currentCard = null;
                        }
                        if (hit.collider != null && TM.IsMyturn() && hit.collider.gameObject.tag == "ClosedCard")
                        {

                            CM.RPC_ReQuest_DrawCard(TM.CurrentTurnIdx);
                        }
                    }
                    else
                    {
                        if (currentCard != null)
                        {
                            currentCard.GetComponent<CardInteraction>().ReduceCard();
                            currentCard.GetComponent<CardInteraction>().Invoke("SetLayerForInvoke", 0.3f);
                            currentCard = null;
                            isEnlargeCard = false;
                        }
                    }
                }


                if (currentCard != null)
                {
                    if (currentCard.GetComponent<Card>().isActiveState == true)
                    {
                        var cardInterection = currentCard.GetComponent<CardInteraction>();
                        switch (touch.phase)
                        {
                            case TouchPhase.Moved:
                                if (cardInterection.IsEnlargeCard == false)
                                    cardInterection.DragCardToTouch(touch);
                                break;
                            case TouchPhase.Stationary:
                                time += Time.deltaTime;
                                if (time >= 2)
                                {
                                    time = -100;
                                    cardInterection.CardExplanation();
                                    isEnlargeCard = true;
                                }
                                break;
                            case TouchPhase.Ended:
                                time = 0f;
                                cardInterection.IsDrop = cardInterection.IsEnlargeCard ? false : true;

                                break;
                            default:
                                Debug.Log("디폴트");
                                break;
                        }
                    }

                }
            }
        }
       
    }
    private void ModulateCardHandSize(bool isEnlarge) // true면 카드패 확대, false면 축소
    {
        if (isEnlarge)
        {
            CM.EnlargeMyCardHand();
        }
        else
            CM.ReduceMyCardHand();
    }
}
