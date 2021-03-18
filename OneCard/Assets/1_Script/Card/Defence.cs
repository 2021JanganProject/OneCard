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
        IsOpenedCardNumber2();
    }

    private void RemoveAttackCount()
    {
        cardManager.CurrentAttackCount = 0;
    }

    private void IsOpenedCardNumber2()
    {
        if (cardManager.CurrentAttackCount > 1)
        {
            if (cardManager.OpenedCard.GetComponent<Card>().cardData.number == 1)
            {
                isActiveState = true;
            }
            else
            {
                isActiveState = false;
            }
        }
    }
}
