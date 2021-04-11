using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 역할 : 카드패 확대 했을 때(만) 카드 하나 하나 마다 가운대로 움직이거나 드래그 해서 내는 동작을 맡음
public class PlayCard : MonoBehaviour
{
    public Vector3 OriginPos { get => originPos; set => originPos = value; }
    public Quaternion OriginRot { get => originRot; set => originRot = value; }
    public bool IsCardEnlarge { get => isCardEnlarge; set => isCardEnlarge = value; }
    public bool IsDrop { get => isDrop; set => isDrop = value; }
    public bool IsEnlarge { get => isEnlarge; set => isEnlarge = value; }

    public void SetEnlarge(bool isEnlarge)
    {
        this.IsEnlarge = isEnlarge;
    }
    [SerializeField] private bool isEnlarge = false;
    [SerializeField] private bool isCardEnlarge = false;
    private float distance = 11.89f;
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Quaternion originRot;

    private bool isDrag = false;
    [SerializeField] CardManager CM;
    private float time = 0f;
    [SerializeField] private bool isDrop = false;
    private SpriteRenderer spriteRenderer;
   
    private void Awake()
    {
        
        CM = CardManager.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
       
        
    }
    private void OnMouseDrag()
    {
        
        time += Time.deltaTime;
        Debug.Log(time);
        if (IsEnlarge == true)
        {
            if (time >= 2)
            {
                transform.gameObject.layer = 9;
                EnlargeCard();
                Debug.Log("호호");
                time = 0f;
                return;  
            }
        }
        if(IsEnlarge == true)
        {
            if(isCardEnlarge == false)
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
                Vector3 cardPos = Camera.main.ScreenToWorldPoint(mousePos);
                transform.position = cardPos;
                
            }   
        }
    }
    /*private void OnMouseDown()
    {
        if (isCardEnlarge == true)
        {
            ReduceCard();
        }
    }*/
    private void OnMouseUp()
    {
        time = 0f;
        
        IsDrop = true;
        if (isCardEnlarge == true)
        {
            ReduceCard();
            Invoke("SetLayer", 0.3f);
        }
        //Invoke("CallReturnOriginPos", 0.3f);
        //CallReturnOriginPos();
    } 
    private void SetLayer()
    {
        transform.gameObject.layer = 0;
    }
    public void EnlargeCard()
    {
        if(IsEnlarge == true)
        {
            IsCardEnlarge = true;
            CM.MoveCardToEnlargeArea(transform);
        }
        
    }
    public void ReduceCard()
    {
        if(IsEnlarge == true)
        {
            CM.MoveCardToOrigin(transform, originPos, originRot);
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke("SetPosAndRot", 0.3f);
            Invoke("SetOnCollider", 0.3f);
            isCardEnlarge = false;
        }
    }
    private void SetOnCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void SetPosAndRot()
    {
        originPos = transform.position;
        originRot = transform.rotation;
        Debug.Log(originPos);
        Debug.Log(originRot);
    }
    public void SetIsDrop()
    {
        isDrop = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDrop == true)
        {
                if (collision.gameObject.tag == "OpenedCard")
                {
                      isDrop = false;
                      isEnlarge = false;
                      if (CM != null)
                      {
                            int index = spriteRenderer.sortingOrder - 1;
                            CM.RemoveCard(index);
                            CM.OpenedCardDeck.Add(transform.gameObject);
                            transform.parent = collision.transform;
                            CM.MoveCardToOpenedCardBase(transform);
                            spriteRenderer.sortingOrder = CM.OpenedCardLayerOrder++;
                            spriteRenderer.sortingLayerName = "CardOnField";
                            CM.EnlargeMyCardHand();
                            CM.SetCardOrderFuck();
                            return;
                      }
                }
            else
            {
                ReduceCard();
                Invoke("SetIsDrop", 0.3f);
            }
        }
   
    }
}


