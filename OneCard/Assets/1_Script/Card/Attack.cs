﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card
{
    [SerializeField] private int attackCount;
    private CardManager cardManager;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
    }


    protected override void Put()
    {
        Debug.Log("오버라이딩");
        base.Put();
        AddAttackCount();
        Debug.Log(cardManager.CurrentAttackCount);
    }

    protected override void Checking()
    {
        base.Checking();
        AttackCheck();
    }

    void AttackCheck()
    {
        if (cardData.number == 1)
        {
            // 공격받았을때 A면 카드를 못낸다.
            if (cardManager.CurrentAttackCount > 1)
            {
                isActiveState = false;
            }
        }
    }

    private void AddAttackCount()
    {
        // attackCount만큼 어딘가에 있을 currentAttackCount에 추가
        switch (cardData.number)
        {
            case 0:
                attackCount = 3;
                break;
            case 1:
                attackCount = 2;
                break;
            default:
                break;
        }

        cardManager.CurrentAttackCount += attackCount;
    }
}