using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImageSlot : MonoBehaviour
{
    private MainProfile mainProfile;
    private UIMain uiMain;
    private Image profileImage;
    
    [SerializeField] private GameObject frameObj; // 테두리 오브젝트

    [SerializeField] private Sprite imageSprite;
    [SerializeField] private Sprite lockSprite;
  
    private bool isOpen = false;

    private int dataNum;

    [SerializeField] private bool isCurrentSelect = false; // 현재 선택된 프로필 사진 테두리 표시를 위한 체크

    private float lastClickTime;
    private float clickInterval = 0.5f;

    public Image ProfileImage { get => profileImage; set => profileImage = value; }
    public bool IsCurrentSelect { get => isCurrentSelect; set => isCurrentSelect = value; }
    public Sprite ImageSprite { get => imageSprite; set => imageSprite = value; }

    private void Awake()
    {
        mainProfile = FindObjectOfType<MainProfile>();
        dataNum = transform.GetSiblingIndex();
        uiMain = FindObjectOfType<UIMain>();
        profileImage = GetComponent<Image>();
        frameObj = transform.GetChild(0).gameObject;
        frameObj.SetActive(false);
      
    }
    private void Start()
    {
        LoadData_IsOpen();
        LoadData_IsCurrentSelect();
        if (isCurrentSelect)
        {
            frameObj.SetActive(true);
        }
        profileImage.sprite = isOpen ? imageSprite : lockSprite;
    }
    public void LoadData_IsOpen()
    {
        string data = PlayerPrefs.GetString($"isOpen{dataNum}", "true");
        
        isOpen = System.Convert.ToBoolean(data);
        
    }
    public void SaveData_IsOpen()
    {
        PlayerPrefs.SetString($"isOpen{dataNum}", $"{isOpen}");
    }
    public void LoadData_IsCurrentSelect()
    {
        string data = PlayerPrefs.GetString($"isCurrentSelect{dataNum}", "false");
        isCurrentSelect = System.Convert.ToBoolean(data);
        Debug.Log(isCurrentSelect);
    }
    public void SaveData_IsCurrentSelect()
    {
        PlayerPrefs.SetString($"isCurrentSelect{dataNum}", $"{isCurrentSelect}");
        Debug.Log(isCurrentSelect);
    }
    public void BtnEvt_SelectProfileImage()
    {
        SelectProfileImage();
    }
    public void SelectProfileImage()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= clickInterval)
        {
            //더블 클릭
            //프로필 이미지 교체
            if (isOpen)
            {
                uiMain.ChangeProfileImage(this);                
            }
        }       
        lastClickTime = Time.time;
    }   
   
}
