using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITimer : MonoBehaviour
{
    [SerializeField] private Text timeText;

    [SerializeField] private float maxTime = 15f;

    [SerializeField] private GameObject timeOver;

    Vector3[] wayPoints;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
        wayPoints[0] = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z);
        wayPoints[1] = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if(currentTime > 0)
        {
            int time = Mathf.CeilToInt(currentTime);
            timeText.text = time.ToString() + "√ ";
        }
        else
        {
            timeText.gameObject.SetActive(false);
            timeOver.SetActive(true);
        }
        if(currentTime < 10)
        {
            StartCoroutine(MoveTimer());
        }
    }
    IEnumerator MoveTimer()
    {
        while(currentTime > 0)
        {
            transform.DOPath(wayPoints, 1f);

            yield return new WaitForSeconds(5f);

        }
    }
}
