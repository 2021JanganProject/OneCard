using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card
{
    [SerializeField]
    private int attackCount;

    void Start()
    {
        Card card = this.gameObject.GetComponent<Card>();
        switch(card.currnetNum)
        {
            case 0:
                attackCount = 3;
                break;
            case 1:
                attackCount = 2;
                break;            
        }
    }

    protected override void Put()
    {
        AddAttackCount();
    }

    protected override void Checking()
    {
        
        base.Checking();
    }

    private void AddAttackCount()
    {
        // attackCount만큼 어딘가에 있을 currentAttackCount에 추가
    }
}
