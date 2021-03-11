using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : Card
{

    protected override void Put()
    {
        ReverseOrderDirection();
    }

    protected override void Checking()
    {
        base.Checking();
    }

    private void ReverseOrderDirection()
    {

    }
}
