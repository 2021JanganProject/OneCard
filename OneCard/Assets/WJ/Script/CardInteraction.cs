using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���� : ī���� Ȯ�� ���� ��(��) ī�� �ϳ� �ϳ� ���� ������ �����̰ų� �巡�� �ؼ� ���� ������ ����
public class CardInteraction : MonoBehaviour
{
    public bool IsEnlargeCard { get => isEnlargeCard; set => isEnlargeCard = value; }
    public bool IsDrop { get => isDrop; set => isDrop = value; }
    public bool IsEnlargeCardHand { get => isEnlargeCardHand; set => isEnlargeCardHand = value; }

    [SerializeField] private bool isEnlargeCardHand = false; //ī���а� Ȯ�ǵ� �������� üũ
    [SerializeField] private bool isEnlargeCard = false; //ī�尡 Ȯ��� ����(�� ������ Ȯ��Ǵ� ����) ���� üũ
    [SerializeField] private Vector3 originPos; // ī���� ��ġ�� ����� ����
    [SerializeField] private Quaternion originRot; // ī���� ȸ������ ����� ����
    [SerializeField] private CardManager CM;

    //Card card;

    private float time = 0f; // ī�尡 �� ���� �ð��� üũ�� ����
    private bool isDrop = false; // ���콺�� ���� �� ��� true�� ��
    private SpriteRenderer spriteRenderer; //ī����� ���ÿ���, ���÷��̾� ������ ���� ����

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
    private void OnMouseDrag() // ī�带 �巡������ �� ī���� Ȯ��/�巡�� ��ġ�� ī�� �̵���Ŵ 
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
    private void OnMouseUp()// ���콺�� ���� �� �巡�� �ð� �ʱ�ȭ/ ī�尡 Ȯ��� ���¶�� ī�� ���
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
    private void OnTriggerStay2D(Collider2D collision) // ���콺�� ���� �� ī��� �ٸ� �ݶ��̴��� �浹 üũ
    {                                                  // �浹�� �ݶ��̴��� OpenedCard�� ī�带 ��, �ƴϸ� ī�带 �ٽ� ī���з�                                                 
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
                    Invoke("SetIsDropForInvoke", 0.3f); //DOTween �Լ� ���� �Ŀ� ����ǵ��� �κ�ũ�� ȣ��
                }
            }
        }
            
    }
    public void DragCardToTouch(Touch touch) //ī�� �巡�� �� ī���� ��ġ�� ��ġ�� �հ��� ��ġ�� ������ ��
    {
        Vector3 touchPos = new Vector3(touch.position.x, touch.position.y, 11.89f);
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(touchPos);
        Quaternion Rot = Quaternion.Euler(0, 0, 0);
        transform.position = cardPos;
        transform.rotation = Rot;
    }
    public void SetLayerForInvoke() // Ȯ���(���� ����) ī�尡 �ٽ� ����ġ�� ���ƿ��� �� ���̾� �缳��
    {
        transform.gameObject.layer = 0;
        Debug.Log("ȣ��~");
    }
    public void ReduceCard() // �巡������ ī��, Ȯ�� ������ ī�带 ī���п��� �ڽ��� ���� ��ġ�� �̵���Ŵ
    {
        if (IsEnlargeCardHand == true)
        {
            CM.MoveCardToOrigin(transform, originPos, originRot);
            GetComponent<BoxCollider2D>().enabled = false; // ����ġ�� ���ư��� �߿� ��ġ�� ���ϰ� collider ��Ȱ��ȭ
            Invoke("SetPosAndRot", 0.3f); //DOTween �Լ��� ���� �� �����Ű�� ���� �κ�ũ
            Invoke("SetOnColliderForInvoke", 0.3f); //DOTween �Լ��� ���� �� �����Ű�� ���� �κ�ũ
            isEnlargeCard = false;
        }
    }
    public void SetOnColliderForInvoke() // ī���� �ݶ��̴��� �ٽ� Ȱ��ȭ��Ŵ/ �κ�ũ�� ȣ���Ͽ� ī�尡 ���ƿ��� �߿� ��ġ ����
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void CardExplanation() // ī�带 �� ������ �� ���̾� �缳�� �� ī��Ȯ�� ȣ��
    {                               // �ش� �缳���� ���̾�� OpenedCard�� �浹�� ����(ī�带 Ȯ���ߴµ� ������ ���� ��������)
        transform.gameObject.layer = 9;
        EnlargeCard();
        Debug.Log("ȣȣ");
    }
    public void SetTrueIsEnlargeCardHand()
    {
        this.isEnlargeCardHand = true;
    }
    public void SetFalseIsEnlargeCardHand()
    {
        this.isEnlargeCardHand = false;
    }
    public void SetPosAndRot() //ī���� ��ġ�� ȸ������ ��������(���콺�̺�Ʈ ���� �� ī������ �ڱ� ��ġ�� �ٽ� ���ƿ��� ���ؼ�)
    {
        originPos = transform.position;
        originRot = transform.rotation;
        Debug.Log(originPos);
        Debug.Log(originRot);
    }
    private void PlayCard(Collider2D collision) // ī�带 ��. �� ī���п��� �ش�ī�� ����, OpenedCard�� �߰�,�̵�/ ���̾� �缳�� 
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
   
    private void EnlargeCard() // ī�带 Ȯ�� ����(���� ����)���� �̵���Ŵ
    {
        if(IsEnlargeCardHand == true)
        {
            IsEnlargeCard = true;
            CM.MoveCardToEnlargeArea(transform);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
   
    private void SetIsDropForInvoke() // ���콺�� ���� isDrop�� true�� �� ���� ��Ȳ�� ���� �ٽ� false�� �ٲ���
    {
        isDrop = false;
    }
    
   
}


