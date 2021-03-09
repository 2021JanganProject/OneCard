using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    // maxTime
     
    private Transform clockHandTransform; // 초침 트랜스폼 넣을 변수
    [SerializeField]
    private float maxTime = 12;
    private float currentTime; // 인게임시간 / 상수값 넣을 변수 (버튼 눌렀을시 0으로 초기화하려고 public 선언함)

   

    private void Awake()
    {
        maxTime = 12f;
        Debug.Log(-0.01f * 360f);
        clockHandTransform = transform.Find("clockHand");// 초침 넣어줌
        
    }
    private void Update()
    {
        maxTime -= Time.deltaTime;

        if (maxTime > 0) // 턴 시간 끝나면 초침 멈추게하려고 조건문에 넣음
        {
            SecManager();
        }
       
        
    }

    public bool TimeOver()
    {
        if (getTime() <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float getDay()
    {
        return currentTime;
    }
    public float getTime()
    {
        return maxTime;
    }
    public void setTime(float time)
    {
        this.maxTime = time;
    }
    private void SecManager()
    {
        currentTime += Time.deltaTime / maxTime; // 프레임에따른 인게임 시간 나누기 상수값(10)한 값을 계속 더해줌
        //첨에 0.1초 지났다치면 day = 0.01임
        // day % 1f = 0.01 그대로
        // -0.01 *360 (-3.6)만큼 z축 회전함
        //이런식으로 tm.time이 0보다 클때까지 쭉감

        float dayNormalized = currentTime % 1f; // 플로트변수에 나머지값 받아옴 
        float rotationDegreesPerDay = 360f; // 360도로 회전할라고 지정
        clockHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay);//실제 초침 z축 회전각 돌려줌
        
    }
    public void resetTimeAndDay()
    {
        maxTime = 12f;
        currentTime = 0;
    }
}
