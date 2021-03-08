using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 턴을 관리 

public class PlayerProfileManager : MonoBehaviour
{
    ClockUI clockUI;
    
    [SerializeField]
    GameObject[] playerProfile;

    [SerializeField]
    Profile[] profile;

    int startTurn;
    int maxCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        startTurn = Random.Range(0, profile.Length);
        clockUI = FindObjectOfType<ClockUI>();

        for (int i = 0; i < playerProfile.Length; i++)
        {
            profile[i] = playerProfile[i].GetComponent<Profile>();
        }

        profile[startTurn].myTurn = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (clockUI.TimeOver())
        {
            float time = clockUI.getTime();
            TurnManager();
            time = 12f;
            clockUI.setTime(time);
        }
    }
    //
    void TurnManager()
    {
        Debug.Log(startTurn);
        profile[startTurn].myTurn = false;
        if(startTurn == 3)
        {
            startTurn = 0;
            profile[startTurn].myTurn = true;
        }
        else
        {
            profile[++startTurn].myTurn = true;
        }
    }
}
