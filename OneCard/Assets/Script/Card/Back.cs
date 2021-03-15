using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : Card
{
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    protected override void Put()
    {
        ReverseOrderDirection();
        base.Put();        
    }

    protected override void Checking()
    {
        base.Checking();
    }

    private void ReverseOrderDirection()
    {
        gameManager.ReverseTurn();
    }
}
