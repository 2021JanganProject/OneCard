using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Image timeBoad;

    [SerializeField] private float maxTime = 15f;

    [SerializeField] private GameObject timeOver;

    //private TurnManager TM;
    private Color viberateColor;
    private Color originColor;
    private float rightMax = 1.0f;
    private float leftMax = -1.0f;
    private float currentPos;
    [SerializeField] private float direction = 20.0f; //떨리는 속도
    [SerializeField] private float viberateTime = 10f; //시계가 떨리는 시점 설정
    private Vector3 originPos;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
        viberateColor = new Color(1, 0.48f, 0.48f, 1);
        originColor = timeBoad.color;
        currentTime = maxTime;
        currentPos = transform.position.x;
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if(currentTime > 0)
        {
            int time = Mathf.CeilToInt(currentTime);
            timeText.text = time.ToString() + "초";
        }
        else
        {
            timeText.gameObject.SetActive(false);
            timeOver.SetActive(true);
        }
        if(currentTime < viberateTime && currentTime >= 0)
        {
            timeBoad.color = viberateColor;
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
        if (currentTime <= 0)
        {
            transform.position = originPos;
            ResetTimerForInvoke(); //@rework
        }
            
    }
    private void ResetTimerForInvoke()
    {
        timeBoad.color = originColor;
        currentTime = maxTime;
        timeText.gameObject.SetActive(true);
        timeOver.SetActive(false);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 10.0f, transform.position.z - 30.0f);
        EffectManager.Instance.PlayEffect(4, pos, transform.position);

        TurnChange();
    }
    private void TurnChange()
    {
        Debug.Log("턴넘김");
    }
}
