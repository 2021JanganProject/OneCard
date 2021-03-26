using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//필요한 거 
//맥스카운트 = 16, 공격 실행됬을 때 어택카운트 초기화/ 방어카드 나와도 어택카운트 초기화
public class AttackCounter : MonoBehaviour
{
    private int currentAttackCount = 0;
    private int maxAttackCount = 16;
    
    [SerializeField] private GameObject[] CountImage;

    //15일때 2,3,5만큼 공격했을때 1만채워지는거 있어야함

    public void SetAttackCount (int attackCount)
    {
        
        if(currentAttackCount + attackCount <= maxAttackCount)
        {  
            for (int i = currentAttackCount; i < currentAttackCount + attackCount; i++)
            {
                CountImage[i].SetActive(true);
            }
            currentAttackCount += attackCount;
        }
        else if(currentAttackCount + attackCount >= maxAttackCount)
        {   
            for (int i = currentAttackCount; i < maxAttackCount; i++)
            {
                CountImage[i].SetActive(true);
            }
            currentAttackCount = maxAttackCount;
        }
    }
    private void ClearAttackCount()
    {
        Debug.Log(currentAttackCount);
        for (int i = 0; i < currentAttackCount; i++)
        {
            CountImage[i].SetActive(false);
        }
        currentAttackCount = 0;
    }
    public void BtnEvt_ClearAttackCountForTest()
    {
        ClearAttackCount();
    }
    public void BtnEvt_attackCount2UpForTest()
    {
        SetAttackCount(2);
    }
    public void BtnEvt_attackCount3UpForTest()
    {
        SetAttackCount(3);
    }
}
