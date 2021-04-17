using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] private Text timeText;

    [SerializeField] private float maxTime = 15f;

    [SerializeField] private GameObject timeOver;

    private float rightMax = 1.0f;
    private float leftMax = -1.0f;
    private float currentPos;
    [SerializeField] private float direction = 20.0f; //떨리는 속도

    private Vector3 originPos;
    private float currentTime;
    private float viberateTime = 10f; //시계가 떨리는 시점 설정
    // Start is called before the first frame update
    void Start()
    {
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
            transform.position = originPos;
    }
}
