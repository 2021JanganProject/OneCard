using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Smoke : Card
{

    protected override void Checking()
    {
        base.Checking();
    }

    protected override void Put()
    {
        base.Put();
        Debug.Log("Put! Smoke!");
    }


}
