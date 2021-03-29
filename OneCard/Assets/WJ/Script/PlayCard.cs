using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard : MonoBehaviour
{
    public void SetEnlarge(bool isEnlarge)
    {
        this.isEnlarge = isEnlarge;
    }
    bool isEnlarge = false;
    float distance = 11.89f;
    private void OnMouseDrag()
    {
        Debug.Log("Drag");
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
    }
    private void OnMouseOver()
    {
        if(isEnlarge == true)
        {
            Vector3 enlargePos = new Vector3(transform.position.x, transform.position.y, 0f);
            transform.position = enlargePos;
        } 
    }
    private void OnMouseExit()
    {
        if(isEnlarge == true)
        {
            Vector3 enlargePos = new Vector3(transform.position.x, transform.position.y, 1.89f);
            transform.position = enlargePos;
        }
    }
}
