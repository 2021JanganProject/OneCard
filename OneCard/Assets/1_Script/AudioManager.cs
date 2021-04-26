using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioclip;
    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }
    #region == BtnEvts ==  
    public void BtnEvt_Audio()
    {
        // ����� ������Ʈ ����
        SoundManager.Instance.CreateDefaultAudioSource();
        // ����� �ҽ� ���
        SoundManager.Instance.Regist(1, audioclip[0]);
        SoundManager.Instance.Regist(2, audioclip[1]);
        SoundManager.Instance.Regist(3, audioclip[2]);
    }

    public void BtnEvt_AudioPlay()
    {
        // ����� ����(ket��, �ݺ�����)
/*        SoundManager.Instance.PlayerAudioClip(1, false);
        SoundManager.Instance.PlayerAudioClip(2, false);*/
        SoundManager.Instance.PlayerAudioClip(3, true);
    }
    #endregion


}
