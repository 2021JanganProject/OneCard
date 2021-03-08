using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
      
        Base = transform.FindChild("ProfileBase").gameObject;
        if(info != null)
            setProfile(info.PlayerName, info.rank.ToString(), info.PlayerImage);
        
    }

    public void setProfile(String name, String rank, Sprite photo)
    {

        this.name.text = name;
        this.rank.text = rank;
        this.photo.sprite = photo;
        
        
    }
    void SetTurnCheck()
    {
        
        if(myTurn != false)
        {
            Image image = GetComponent<Image>();
            Color color = image.color;
            color.a = 1.0f;
            image.color = color;
        }
        else
        {
            Image image = GetComponent<Image>();
            Color color = image.color;
            color.a = 0.4f;
            image.color = color;
        }
    }
    // Update is called once per frame
    void Update()
    {
        SetTurnCheck();
    }
}
