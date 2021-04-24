using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    [SerializeField] private GameObject setting;

    private bool isSetting = false;

    // Start is called before the first frame update
    void Start()
    {
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
}
