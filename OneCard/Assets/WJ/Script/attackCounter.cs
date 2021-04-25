using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
//필요한 거
//맥스카운트 = 16, 공격 실행됬을 때 어택카운트 초기화/ 방어카드 나와도 어택카운트 초기화
public class AttackCounter : MonoBehaviourPun
{
    [SerializeField] private int currentAttackCount = 0;
    private int maxAttackCount = 12;
    private float currentRot = 0f;
    [SerializeField] private GameObject[] CountImage;
    [SerializeField] private Sprite spinCylinder;
    [SerializeField] private Transform revolverBase;
    [SerializeField] private RectTransform baseTransform;
    [SerializeField] private float direction = 20.0f; //떨리는 속도

    private float rightMax = 1.0f;
    private float leftMax = -1.0f;
    private float currentPos;

    private Vector3 originPos;
    public int CurrentAttackCount { get => currentAttackCount; set => currentAttackCount = value; }

    private void Start()
    {
        currentPos = revolverBase.position.x;
        originPos = revolverBase.position;
    }
    public void RPC_ALL_SetAttackCount(int attackCount)
    {
        photonView.RPC(nameof(SetAttackCount), RpcTarget.AllBufferedViaServer, attackCount);
    }
    [PunRPC]
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
        RPC_ALL_StartAttack();
    }
    public void RPC_ALL_StartAttack()
    {
        photonView.RPC(nameof(StartAttack), RpcTarget.AllViaServer);
    }
    [PunRPC]
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
            Vector3 ImagePos = new Vector3(CountImage[i].transform.position.x, CountImage[i].transform.position.y, CountImage[i].transform.position.z - 30.0f);
            EffectManager.Instance.PlayEffect(2, ImagePos, CountImage[i].transform.position);
            EffectManager.Instance.PlayEffect(0, this.transform.position, this.transform.position);
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
            Vector3 ImagePos = new Vector3(CountImage[i].transform.position.x, CountImage[i].transform.position.y, CountImage[i].transform.position.z - 30.0f);
            EffectManager.Instance.PlayEffect(2, ImagePos, CountImage[i].transform.position);
            EffectManager.Instance.PlayEffect(0, this.transform.position, this.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
        currentAttackCount = 0;
        baseTransform.rotation = Quaternion.Euler(0, 0, 0);
        currentRot = 0f;
    }
    IEnumerator Attack(int count)
    {
        VibrateRevolver(revolverBase);
        for (int i = count; i < currentAttackCount; i++)
        {
            CountImage[i].SetActive(false);
            currentRot += 60f;
            baseTransform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, currentRot), 0.05f);
            Vector3 ImagePos = new Vector3(CountImage[i].transform.position.x, CountImage[i].transform.position.y, CountImage[i].transform.position.z - 30.0f);
            EffectManager.Instance.PlayEffect(2, ImagePos, CountImage[i].transform.position);
            EffectManager.Instance.PlayEffect(0, this.transform.position, this.transform.position);
            //revolverBase.DOMove(new Vector3(revolverBase.position.x - 1, revolverBase.position.y, revolverBase.position.z), 0.02f);//
            //revolverBase.DOMove(new Vector3(revolverBase.position.x + 1, revolverBase.position.y, revolverBase.position.z), 0.02f);
            yield return new WaitForSeconds(0.2f);
        }
        MoveOrigin(revolverBase);
        currentAttackCount = count;
        baseTransform.rotation = Quaternion.Euler(0, 0, 0);    
        currentRot = 0f;
    }
    public void BtnEvt_attackCount2UpForTest()
    {
        //SetAttackCount(2);
        RPC_ALL_SetAttackCount(2);
    }
    public void BtnEvt_attackCount3UpForTest()
    {
        //SetAttackCount(3);
        RPC_ALL_SetAttackCount(3);
    }
    private void VibrateRevolver(Transform transform)
    {
        currentPos += Time.deltaTime * direction;
        if (currentPos >= rightMax)
        {
            direction *= -1;
            currentPos = rightMax;
        }
        else if (currentPos <= leftMax)
        {
            direction *= -1;
            currentPos = leftMax;
        }
        transform.position = new Vector3(currentPos, transform.position.y, transform.position.z);
  
    }
    private void MoveOrigin(Transform transform)
    {
        transform.position = originPos;
    }
}
