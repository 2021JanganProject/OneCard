using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card
{
    [SerializeField]
    private int attackCount;
    [SerializeField]
    public Card card;
    private CardManager cardManager;

    void Start()
    {
        card = GetComponent<Card>();
        switch (card.currnetNum)
        {
            case 0:
                attackCount = 3;
                break;
            case 1:
                attackCount = 2;
                break;
            default:
                return;
        }
        
    }


    protected override void Put()
    {
        Debug.Log("오버라이딩");
        base.Put();
        AddAttackCount();
    }

    protected override void Checking()
    {
        base.Checking();
        AttackCheck();
    }

    void AttackCheck()
    {
        if (card.currnetNum == 1)
        {
            // 공격받았을때 A면 카드를 못낸다.
            if (cardManager.currentAttackCount > 1)
            {
                card.isActiveState = false;
            }
        }
    }

    private void AddAttackCount()
    {
        // attackCount만큼 어딘가에 있을 currentAttackCount에 추가
        cardManager.currentAttackCount += attackCount;
    }
}
