using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeText;
    public float time;

    
    // Start is called before the first frame update
    private void Awake()
    {
        time = 12f;
        
    }
   

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
            Timer();
        
    }

    void Timer()
    {
        if (time > 0)
            time -= Time.deltaTime;

        timeText.text = Mathf.Ceil(time).ToString();

        if (time <= 0)
            timeText.text = "턴 종료";
    }
    
}
