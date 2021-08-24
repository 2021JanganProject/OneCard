using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject profileImagePopUp;
    [SerializeField] private GameObject playerInfoObj;
    
    private bool isSetting = false;
    private MainProfile mainProfile;
    [SerializeField] private ProfileImageSlot[] profileImageSlots;

    private int currentProfileImage = 0;

    private void Awake()
    {
        LoadData_ProfileImage();
        mainProfile = FindObjectOfType<MainProfile>();
        profileImageSlots = profileImagePopUp.transform.GetComponentsInChildren<ProfileImageSlot>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        mainProfile.ProfileImage.sprite = profileImageSlots[currentProfileImage].ImageSprite;
        if (!isSetting)
            setting.SetActive(false);
    }

    public void BtnEvt_OnSetting()
    {
        if(isSetting == false)
            Setting();
    }
    private void Setting()
    {
        isSetting = !isSetting;
        setting.SetActive(isSetting);
    }
    
    public void BtnEvt_OffSetting()
    {
        if (isSetting == true)
            Setting();            
    }
    public void BtnEvt_ShowProfileImagePopUp()
    {
        if (!profileImagePopUp.activeSelf)
        {
            profileImagePopUp.SetActive(true);
        }
        
    }
    public void BtnEvt_CloseProfileImagePopUp()
    {
        if (profileImagePopUp.activeSelf)
        {
            profileImagePopUp.SetActive(false);
        }
    }
    public void BtnEvt_ShowPlayerInfo()
    {
        if (!playerInfoObj.activeSelf)
        {
            playerInfoObj.SetActive(true);
        }
    }
    public void BtnEvt_ClosePlayerInfo()
    {
        if (playerInfoObj.activeSelf)
        {
            playerInfoObj.SetActive(false);
        }
    }
    public void ChangeProfileImage(ProfileImageSlot slot)
    {
        var sprite = slot.ProfileImage.sprite;
        mainProfile.ProfileImage.sprite = sprite;
        currentProfileImage = slot.transform.GetSiblingIndex();
        SaveData_ProfileImage();
        ManageActiveSlot();
    }
    public void ManageActiveSlot()
    {
        for (int i = 0; i < profileImageSlots.Length; i++)
        {
            if(i == currentProfileImage)
            {
                ActiveSlot(true, i);
            }
            else
            {
                ActiveSlot(false, i);
            }
            profileImageSlots[i].SaveData_IsCurrentSelect();
        }
    }
    public void ActiveSlot(bool isSelect, int i)
    {
        profileImageSlots[i].IsCurrentSelect = isSelect;
        profileImageSlots[i].transform.GetChild(0).gameObject.SetActive(isSelect);     
    }
    public void SaveData_ProfileImage()
    {
        PlayerPrefs.SetInt("CurrentProfileImage", currentProfileImage);
    }
    public void LoadData_ProfileImage()
    {
        int i = PlayerPrefs.GetInt("CurrentProfileImage", 0);
        currentProfileImage = i;
    }
}
