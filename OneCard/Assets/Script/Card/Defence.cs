using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : Card
{

    protected override void Put()
    {
        RemoveAttackCount();
    }

    protected override void Checking()
    {
        base.Checking();
        IsOpenedCardNumber2();
    }

    private void RemoveAttackCount()
    {

    }

    private void IsOpenedCardNumber2()
    {

    }
}
