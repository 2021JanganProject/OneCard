using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//필요한 거 
//맥스카운트 = 16, 공격 실행됬을 때 어택카운트 초기화/ 방어카드 나와도 어택카운트 초기화
public class AttackCounter : MonoBehaviour
{
    private int CurrentAttackCount = 0;
    private int maxAttackCount = 16;
    
    [SerializeField]
    private GameObject[] CountImage;

    //15일때 2,3,5만큼 공격했을때 1만채워지는거 있어야함

    public void SetAttackCount (int attackCount)
    {
        
        if(CurrentAttackCount + attackCount <= maxAttackCount)
        {  
            for (int i = CurrentAttackCount; i < CurrentAttackCount + attackCount; i++)
            {
                CountImage[i].SetActive(true);
            }
            CurrentAttackCount += attackCount;
        }
        else if(CurrentAttackCount + attackCount >= maxAttackCount)
        {   
            for (int i = CurrentAttackCount; i < maxAttackCount; i++)
            {
                CountImage[i].SetActive(true);
            }
            CurrentAttackCount = maxAttackCount;
        }
        

    }
    public void ClearAttackCount()
    {
        Debug.Log(CurrentAttackCount);
        for (int i = 0; i < CurrentAttackCount; i++)
        {
            CountImage[i].SetActive(false);
        }
        CurrentAttackCount = 0;
    }


    public void attackCount2Up()
    {
        SetAttackCount(2);
    }
    public void attackCount3Up()
    {
        SetAttackCount(3);
    }
}
