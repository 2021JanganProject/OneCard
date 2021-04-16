using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeCardHand : MonoBehaviour
{
    public bool IsEnlarge { get => this.isEnlarge; set => isEnlarge = value; }

    private CardManager CM;
    private bool isEnlarge = false;

    
    // Start is called before the first frame update
    void Start()
    {
        CM = CardManager.instance;
    }

    // Update is called once per frame
    void Update()
    { // 0.75  11    5
//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) //마우스 클릭 시 레이를 쏴서 카드패 확대,축소
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("InvisibleWall"));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);
            if(hit.collider != null && hit.collider.transform == transform)
            {
                isEnlarge = true;
                Debug.Log(isEnlarge);
                ModulateCardHandSize(isEnlarge);
                
            }
            else if(hit.collider != null && hit.collider.gameObject.tag == "MyCardEnlarge")
            {
                return;
            }
            else
            {
                isEnlarge = false;
                Debug.Log(isEnlarge);
                ModulateCardHandSize(isEnlarge);
                
            }

            if(hit.collider != null && hit.collider.gameObject.tag == "ClosedCardDeck")
            {
                //@DrawLogic...
            }
        }
//#else
        /*if (Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                int layerMask = (-1) - (1 << LayerMask.NameToLayer("InvisibleWall"));
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);
                if (hit.collider != null && hit.collider.transform == transform)
                {
                    isEnlarge = true;
                    Debug.Log(isEnlarge);
                    ModulateCardHandSize(isEnlarge);

                }
                else if (hit.collider != null && hit.collider.gameObject.tag == "MyCardEnlarge")
                {
                    return;
                }
                else
                {
                    isEnlarge = false;
                    Debug.Log(isEnlarge);
                    ModulateCardHandSize(isEnlarge);

                }
            }
         
        }
//#endif*/
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
