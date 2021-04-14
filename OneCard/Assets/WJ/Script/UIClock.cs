using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIClock : MonoBehaviour
{
    public float CurrentTime
    {
        get
        {
            return currentTime;
        }
        set
        {
            if (currentTime <= 0)
            {
                currentTime = value;
            }
        }
    }
    [SerializeField] private float maxTime = 12f; //시계가 한바퀴 도는데 걸릴 시간 / 
    [SerializeField] private Transform clockHandTransform; // 초침 트랜스폼 넣을 변수
    private RectTransform rectTransform;

    private float currentTimeForClockhand; // 인게임시간 / 상수값 넣을 변수 
    private float currentTime;

    public void BtnEvt_ResetTimeForTest()
    {
        ResetCurrentTimeAndClockhand();
    }
    public void ResetCurrentTimeAndClockhand()
    {
        currentTime = maxTime;
        currentTimeForClockhand = 0;
    }
    private void RotateClockHand()
    {
        currentTimeForClockhand += Time.deltaTime / maxTime; // 프레임에따른 인게임 시간 나누기 상수값(10)한 값을 계속 더해줌
        //첨에 0.1초 지났다치면 day = 0.01임 /day % 1f = 0.01 그대로/ -0.01 *360 (-3.6)만큼 z축 회전함/이런식으로 tm.time이 0보다 클때까지 쭉감

        float timeNormalized = currentTimeForClockhand % 1f; // 플로트변수에 나머지값 받아옴 

        float rotationDegreesPerMaxTime = 360f; // 360도로 회전할라고 지정

        clockHandTransform.eulerAngles = new Vector3(0, 0, -timeNormalized * rotationDegreesPerMaxTime);//실제 초침 z축 회전각 돌려줌 
    }
    private void Awake()
    {
        currentTime = maxTime;
        rectTransform = GetComponent<RectTransform>();
    }
   
    private void Update()
    {
        currentTime -= Time.deltaTime;
        //Debug.Log(currentTime);
        if (currentTime > 0) 
        {
            RotateClockHand();
        }
        if(currentTime <= 0)
        {
            currentTime = maxTime;
        }
        if(currentTime <= 10)
        {
            while(currentTime <= 0)
            {
                rectTransform.DOMoveX(-2, 0.2f);
                rectTransform.DOMoveX(4, 0.3f);
                rectTransform.DOMoveX(2, 0.4f);
            }
            
        }
       
    } 
    IEnumerator MoveClock()
    {
        while (currentTime <= 0)
        {
            rectTransform.DOMoveX(-2, 0.1f);
            rectTransform.DOMoveX(4, 0.1f);
            rectTransform.DOMoveX(2, 0.1f);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
