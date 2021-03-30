using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeCardHand : MonoBehaviour
{
    CardManager CM;
    private GameObject currentCard = null;
    
    private bool isEnlarge = false;
    private bool isCardEnlarge = false;
    // Start is called before the first frame update
    void Start()
    {
        CM = CardManager.instance;
    }

    // Update is called once per frame
    void Update()
    { // 0.75  11    5
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if(hit.collider != null && hit.collider.transform == transform)
            {
                isEnlarge = true;
                Debug.Log(isEnlarge);
                ModulateCardHandSize(isEnlarge);
                
            }
            else if(hit.collider != null && hit.collider.gameObject.tag == "MyCard")
            {  
                if(currentCard != null)
                {
                    if (hit.collider.gameObject == currentCard)
                    {
                        currentCard.GetComponent<PlayCard>().ReduceCard();
                        currentCard = null;
                        return;
                    }
                    else
                    {
                        currentCard.GetComponent<PlayCard>().ReduceCard();
                        hit.collider.gameObject.GetComponent<PlayCard>().EnlargeCard();
                    }
                }
                else 
                {
                    hit.collider.gameObject.GetComponent<PlayCard>().EnlargeCard();
                }
                 currentCard = hit.collider.gameObject;
            }
            else
            {
                isEnlarge = false;
                Debug.Log(isEnlarge);
                if (currentCard != null)
                {
                    currentCard.GetComponent<PlayCard>().ReduceCard();
                    currentCard = null;
                }
                ModulateCardHandSize(isEnlarge);
                
            }
            

        }
    }
    private void ModulateCardHandSize(bool isEnlarge)
    {
        if (isEnlarge)
            CM.EnlargeMyCardHand();
        else
            CM.ReduceMyCardHand();
    }
}
