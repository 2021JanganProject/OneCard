using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 역할 : 카드패 확대 했을 때(만) 카드 하나 하나 마다 가운대로 움직이거나 드래그 해서 내는 동작을 맡음
public class CardInteraction : MonoBehaviour
{
    public bool IsEnlargeCard { get => isEnlargeCard; set => isEnlargeCard = value; }
    public bool IsDrop { get => isDrop; set => isDrop = value; }
    public bool IsEnlargeCardHand { get => isEnlargeCardHand; set => isEnlargeCardHand = value; }

    [SerializeField] private bool isEnlargeCardHand = false; //카드패가 확되된 상태인지 체크
    [SerializeField] private bool isEnlargeCard = false; //카드가 확대된 상태(꾹 눌러서 확대되는 상태) 인지 체크
    [SerializeField] private Vector3 originPos; // 카드의 위치를 기억할 변수
    [SerializeField] private Quaternion originRot; // 카드의 회전값을 기억할 변수
    [SerializeField] private CardManager CM;

    //Card card;

    private float time = 0f; // 카드가 꾹 눌린 시간을 체크할 변수
    private bool isDrop = false; // 마우스를 땟을 때 잠깐 true가 됨
    private SpriteRenderer spriteRenderer; //카드들의 소팅오더, 소팅레이어 변경을 위한 변수

    private void Awake()
    {
        //card = GetComponent<Card>();
        CM = CardManager.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }
//#else*/
    private void Update()
    {

    }
    private void OnMouseDrag() // 카드를 드래그했을 때 카드의 확대/드래그 위치로 카드 이동시킴 
    {
        if (GetComponent<Card>().isActiveState)
        {
            time += Time.deltaTime;
            //Debug.Log(time);
            if (isEnlargeCardHand == true)
            {
                if (time >= 2)
                {
                    CardExplanation();
                    isEnlargeCard = true;
                    time = 0;
                }
            }
            if (isEnlargeCardHand == true)
            {
                if (isEnlargeCard == false)
                {
                    DragCardToMouse();
                }
            }
        }

    }
    private void OnMouseUp()// 마우스를 땠을 때 드래그 시간 초기화/ 카드가 확대된 상태라면 카드 축소
    {
        if (GetComponent<Card>().isActiveState)
        {
            time = 0f;
            if (isEnlargeCard == true)
            {
                isDrop = false;
                ReduceCard();
                Invoke("SetLayerForInvoke", 0.3f);
            }
            else
            {
                IsDrop = true;
            }
        }
    }
    //#endif
    private void OnTriggerStay2D(Collider2D collision) // 마우스를 땠을 때 카드와 다른 콜라이더의 충돌 체크
    {                                                  // 충돌한 콜라이더가 OpenedCard면 카드를 냄, 아니면 카드를 다시 카드패로                                                 
        if (GetComponent<Card>().isActiveState)
        {
            
            if (isDrop == true)
            {
                if (collision.gameObject.tag == "OpenedCard")
                {
                    isDrop = false;
                    isEnlargeCardHand = false;
                    if (CM != null)
                    {
                        PlayCard(collision);
                        return;
                    }
                }
                else
                {

                    ReduceCard();
                    Invoke("SetIsDropForInvoke", 0.3f); //DOTween 함수 끝난 후에 실행되도록 인보크로 호출
                }
            }
        }
            
    }
    public void DragCardToTouch(Touch touch) //카드 드래그 시 카드의 위치가 터치한 손가락 위치로 오도록 함
    {
        Vector3 touchPos = new Vector3(touch.position.x, touch.position.y, 11.89f);
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(touchPos);
        Quaternion Rot = Quaternion.Euler(0, 0, 0);
        transform.position = cardPos;
        transform.rotation = Rot;
    }
    public void SetLayerForInvoke() // 확대된(설명 구역) 카드가 다시 원위치로 돌아왔을 때 레이어 재설정
    {
        transform.gameObject.layer = 0;
        Debug.Log("호출~");
    }
    public void ReduceCard() // 드래그중인 카드, 확대 구역의 카드를 카드패에서 자신의 원래 위치로 이동시킴
    {
        if (IsEnlargeCardHand == true)
        {
            CM.MoveCardToOrigin(transform, originPos, originRot);
            GetComponent<BoxCollider2D>().enabled = false; // 원위치로 돌아가는 중에 터치를 못하게 collider 비활성화
            Invoke("SetPosAndRot", 0.3f); //DOTween 함수가 끝난 뒤 실행시키기 위해 인보크
            Invoke("SetOnColliderForInvoke", 0.3f); //DOTween 함수가 끝난 뒤 실행시키기 위해 인보크
            isEnlargeCard = false;
        }
    }
    public void SetOnColliderForInvoke() // 카드의 콜라이더를 다시 활성화시킴/ 인보크로 호출하여 카드가 돌아오는 중에 터치 막음
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void CardExplanation() // 카드를 꾹 눌렀을 때 레이어 재설정 후 카드확대 호출
    {                               // 해당 재설정된 레이어는 OpenedCard와 충돌을 안함(카드를 확대했는데 내지는 것을 방지해줌)
        transform.gameObject.layer = 9;
        EnlargeCard();
        Debug.Log("호호");
    }
    public void SetTrueIsEnlargeCardHand()
    {
        this.isEnlargeCardHand = true;
    }
    public void SetFalseIsEnlargeCardHand()
    {
        this.isEnlargeCardHand = false;
    }
    public void SetPosAndRot() //카드의 위치와 회전값을 설정해줌(마우스이벤트 종료 후 카드패의 자기 위치로 다시 돌아오기 위해서)
    {
        originPos = transform.position;
        originRot = transform.rotation;
        Debug.Log(originPos);
        Debug.Log(originRot);
    }
    private void PlayCard(Collider2D collision) // 카드를 냄. 내 카드패에서 해당카드 제거, OpenedCard에 추가,이동/ 레이어 재설정 
    {
        int index = spriteRenderer.sortingOrder - 1;
        CM.MyCards.RemoveAt(index);
        CM.OpenedCardDeck.Add(transform.gameObject);
        CM.OpenedCard = transform.gameObject;
        CM.UpdateCardData();
        transform.parent = collision.transform;
        CM.MoveCardToOpenedCardBase(transform);
        spriteRenderer.sortingOrder = CM.OpenedCardSortingOrder++;
        spriteRenderer.sortingLayerName = "CardOnField";
        CM.EnlargeMyCardHand();
        CM.CallSetCardSortingOrderAndSortingLayerName();
        
        Debug.Log(CM.OpenedCardDeck.Count);
    }
   
    private void DragCardToMouse()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11.89f);
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(mousePos);
        Quaternion Rot = Quaternion.Euler(0, 0, 0);
        transform.position = cardPos;
        transform.rotation = Rot;
    }
   
    private void EnlargeCard() // 카드를 확대 구역(설명 구역)으로 이동시킴
    {
        if(IsEnlargeCardHand == true)
        {
            IsEnlargeCard = true;
            CM.MoveCardToEnlargeArea(transform);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
   
    private void SetIsDropForInvoke() // 마우스를 때면 isDrop을 true로 한 다음 상황에 따라 다시 false로 바꿔줌
    {
        isDrop = false;
    }
    
   
}


