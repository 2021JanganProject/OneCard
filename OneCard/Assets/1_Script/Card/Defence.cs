using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : Card
{
    private CardManager cardManager;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
    }
    protected override void Put()
    {
        base.Put();
        RemoveAttackCount();
    }

    protected override void Checking()
    {
        base.Checking();
        DefenceCheck();
    }

    private void RemoveAttackCount()
    {
        cardManager.CurrentAttackCount = 0;
    }

    private void DefenceCheck()
    {
        if (cardManager.CurrentCard.cardColor == eCardColor.Black && cardManager.CurrentCard.number == 12)
        {
            isActiveState = true;
        }
    }
}
