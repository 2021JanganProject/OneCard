using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneMore : Card
{

    protected override void Put()
    {
        base.Put();
        OneMoreMyTurn();
    }

    protected override void Checking()
    {
        base.Checking();
    }

    private void OneMoreMyTurn()
    {

    }
}
