using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    private const float REAL_SECONDS_PER_INGAME_DAY = 12f; //상수값 지정 (시계가 한바퀴 도는데 걸릴 시간임)
     
    private Transform clockHandTransform; // 초침 트랜스폼 넣을 변수

    private float day; // 인게임시간 / 상수값 넣을 변수 (버튼 눌렀을시 0으로 초기화하려고 public 선언함)

    private float time; 

    private void Awake()
    {
        time = 12f;
        Debug.Log(-0.01f * 360f);
        clockHandTransform = transform.Find("clockHand");// 초침 넣어줌
        
    }
    private void Update()
    {
        time -= Time.deltaTime;

        if (time > 0) // 턴 시간 끝나면 초침 멈추게하려고 조건문에 넣음
        {
            SecManager();
        }
       
        
    }
    public float getDay()
    {
        return day;
    }
    public float getTime()
    {
        return time;
    }
    public void setTime(float time)
    {
        this.time = time;
    }
    private void SecManager()
    {
        
        
            day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY; // 프레임에따른 인게임 시간 나누기 상수값(10)한 값을 계속 더해줌
            //첨에 0.1초 지났다치면 day = 0.01임
            // day % 1f = 0.01 그대로
            // -0.01 *360 (-3.6)만큼 z축 회전함
            //이런식으로 tm.time이 0보다 클때까지 쭉감

            float dayNormalized = day % 1f; // 플로트변수에 나머지값 받아옴 

            float rotationDegreesPerDay = 360f; // 360도로 회전할라고 지정

            clockHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPerDay);//실제 초침 z축 회전각 돌려줌
        
    }
    public void resetTimeAndDay()
    {
        time = 12f;
        day = 0;
    }
}
