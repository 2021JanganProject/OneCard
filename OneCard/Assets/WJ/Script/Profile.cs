﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 프로필 초기화 및 관련 함수 
public class Profile : MonoBehaviour
{
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text rank;
    [SerializeField]
    private Image photo;
    [SerializeField]
    private GameObject Base;
    [SerializeField]
    private PlayerInfo info;

    
    public bool myTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        //Base = transform.FindChild("ProfileBase").gameObject;
    }

    public void setProfile(PlayerInfo playerInfo)
    {
        this.name.text = playerInfo.nickname;
        this.rank.text = playerInfo.rank.ToString();
        //this.photo.sprite = photo;
    }
    // 
    // => TurnManager
    void SetTurnCheck()
    {
        if(myTurn != false)
        {
            Image image = GetComponent<Image>();
            SetAlpha(image, 1.0f);
        }
        else
        {
            Image image = GetComponent<Image>();
            SetAlpha(image , 0.4f);
        }
    }

    void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    // Update is called once per frame
    void Update()
    {
        SetTurnCheck();
    }
}
