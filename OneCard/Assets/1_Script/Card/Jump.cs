using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Card
{
    private int jumpCount;



    protected override void Put()
    {
        OrderJump();
        base.Put();
    }

    protected override void Checking()
    {
        base.Checking();
    }

    private void OrderJump()
    {

    }
}
