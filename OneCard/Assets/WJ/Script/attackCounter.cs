using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//필요한 거
//맥스카운트 = 16, 공격 실행됬을 때 어택카운트 초기화/ 방어카드 나와도 어택카운트 초기화
public class AttackCounter : MonoBehaviour
{
    [SerializeField] private int currentAttackCount = 0;
    private int maxAttackCount = 12;
    private float currentRot = 0f;
    [SerializeField] private GameObject[] CountImage;
    
    [SerializeField] private RectTransform baseTransform;
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }

    //15일때 2,3,5만큼 공격했을때 1만채워지는거 있어야함

    public void SetAttackCount(int attackCount)
    {
        if (CurrentAttackCount + attackCount <= maxAttackCount)
        {
            for (int i = CurrentAttackCount; i < CurrentAttackCount + attackCount; i++)
            {
                CountImage[i].SetActive(true);
            }
            CurrentAttackCount += attackCount;
        }
        else if (CurrentAttackCount + attackCount >= maxAttackCount)
        {
            for (int i = CurrentAttackCount; i < maxAttackCount; i++)
            {
                CountImage[i].SetActive(true);
            }
            CurrentAttackCount = maxAttackCount;
        }
    }
    public void BtnEvt_StartAttack()
    {
        StartAttack();
    }
    private void StartAttack()
    {
        if(currentAttackCount > 6)
        {
            StartCoroutine(AttackStrong(6));
            
        }
        else
        {
            StartCoroutine(Attack(0)); 
        }
    }
    IEnumerator AttackStrong(int count)
    {
        for (int i = count; i < currentAttackCount; i++)
        {
            CountImage[i].SetActive(false);
            currentRot += 60f;
            baseTransform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, currentRot), 0.05f);
            yield return new WaitForSeconds(0.2f);
        }
        currentAttackCount = count;
        baseTransform.rotation = Quaternion.Euler(0, 0, 0);
        currentRot = 0f;
        for (int i = 0; i < currentAttackCount; i++)
        {
            CountImage[i].SetActive(false);
            currentRot += 60f;
            baseTransform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, currentRot), 0.05f);
            yield return new WaitForSeconds(0.2f);
        }
        currentAttackCount = 0;
        baseTransform.rotation = Quaternion.Euler(0, 0, 0);
        currentRot = 0f;
    }
    IEnumerator Attack(int count)
    {
        for (int i = count; i < currentAttackCount; i++)
        {
            CountImage[i].SetActive(false);
            currentRot += 60f;
            baseTransform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, currentRot), 0.05f);
            yield return new WaitForSeconds(0.2f);
        }
        currentAttackCount = count;
        baseTransform.rotation = Quaternion.Euler(0, 0, 0);
        currentRot = 0f;
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
