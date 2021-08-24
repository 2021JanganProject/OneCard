using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerGoods
{
    public const int MAXIMUM_GOLD = 99999999;
    public const int MAXIMUM_CRYSTAL = 99999999;
    public const int MAXIMUM_HEART = 5;

    public int gold;
    public int crystal;
    public int heart;
}
public class MainProfile : MonoBehaviour
{
    private Text nickName;
    private Image profileImage;
    private int winCount;

    public Image ProfileImage { get => profileImage; set => profileImage = value; }
    public Text NickName { get => nickName; set => nickName = value; }

    private void Awake()
    {
        nickName = transform.Find("NickNamePanel").GetComponentInChildren<Text>();
        profileImage = transform.Find("ProfileImg").Find("ProfileImage").GetComponent<Image>();
    }

}
