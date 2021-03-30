using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard : MonoBehaviour
{
    public Vector3 OriginPos { get => originPos; set => originPos = value; }
    public Quaternion OriginRot { get => originRot; set => originRot = value; }
    public bool IsCardEnlarge { get => isCardEnlarge; set => isCardEnlarge = value; }

    public void SetEnlarge(bool isEnlarge)
    {
        this.isEnlarge = isEnlarge;
    }
    [SerializeField] private bool isEnlarge = false;
    [SerializeField] private bool isCardEnlarge = false;
    private float distance = 11.89f;
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Quaternion originRot;
    
    CardManager CM;

    

    private void Awake()
    {
        CM = CardManager.instance;
    }

    public void EnlargeCard()
    {
        if(isEnlarge == true)
        {
            originPos = transform.position;
            originRot = transform.rotation;
            IsCardEnlarge = true;
            CM.MoveCardToEnlargeArea(transform);
        }
        
    }
    public void ReduceCard()
    {
        if(isEnlarge == true)
        {
            CM.MoveCardToOrigin(transform, originPos, originRot);
            originPos = transform.position;
            originRot = transform.rotation;
        }
    }
    
}
