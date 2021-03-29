using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeCardHand : MonoBehaviour
{
    CardManager CM;

    public void SetCardEnlarge(bool isCardEnlarge)
    {
        this.isCardEnlarge = isCardEnlarge;
    }
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
                isCardEnlarge = true;
            }
            else
            {
                if(isCardEnlarge == false)
                {
                    isEnlarge = false;
                    Debug.Log(isEnlarge);
                    ModulateCardHandSize(isEnlarge);
                }  
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
