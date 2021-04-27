using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // AudioSource의 키값(Key, Value)을 받아 저장하는 변수
    Dictionary<int, AudioClip> oAudioClipsMap = new Dictionary<int, AudioClip>();
    AudioSource oAS_Once = null;
    AudioSource oAS_Loop = null;


    private static SoundManager _instance = null;

    public static SoundManager Instance {
        get
        {
            if(_instance == null)
            {
                _instance = new SoundManager();
            }
            return _instance;
        }
    }

    public void CreateDefaultAudioSource()
    {
        if(oAS_Loop != null && oAS_Once != null)
        {
            Debug.Log("Already Created Dafault AudioSources!");
            return;
        }

        // SoundManager라는 게임 오브젝트를 생성
        GameObject oSoundManager = GameObject.Find("SoundManager");
        {
            oSoundManager = new GameObject("SoundManager");
            Debug.Assert(oSoundManager != null, "Can not create new SoundManager GameeObject");
        }
        GameObject.DontDestroyOnLoad(oSoundManager);

        oAS_Once = oSoundManager.AddComponent<AudioSource>();
        oAS_Once.loop = false;

        oAS_Loop = oSoundManager.AddComponent<AudioSource>();
        oAS_Loop.loop = true;
    }

    // 키값을 등록하는 함수
    public void Regist(int iInAudioKey, AudioClip oInAudioClip)
    {
        Debug.Assert(oInAudioClip != null, "Invalid AudioClip! AudioKey= " + iInAudioKey.ToString());

        // oAudioClipsMap에 iInAudioKey가 존재한다면 등록하지 않고 return
        if (oAudioClipsMap.ContainsKey(iInAudioKey) == true)
        {
            Debug.Log("Already Registed AudioClip! AudioKey= " + iInAudioKey.ToString());
            return;
        }
        
        oAudioClipsMap.Add(iInAudioKey, oInAudioClip);
    }

    public void PlayerAudioClip(int iInAudioKey, bool IsLoop)
    {
        if(oAudioClipsMap.ContainsKey(iInAudioKey) == false)
        {
            Debug.Log("Not exist AudioClip! AudioKey= " + iInAudioKey.ToString());
            return;
        }

        // 반복하여 사용할 AudioSource라면 반복사용
        if (IsLoop)
        {
            Debug.Assert(oAS_Loop != null, "AudioSource is null!");
            oAS_Loop.Stop();
            oAS_Loop.clip = oAudioClipsMap[iInAudioKey];
            oAS_Loop.Play();
        }
        // 한번만 사용할 AudioSource라면 한번만 사용
        else
        {
            Debug.Assert(oAS_Once != null, "AudioSource is null!");
            oAS_Once.PlayOneShot(oAudioClipsMap[iInAudioKey]);
        }
    }
}
