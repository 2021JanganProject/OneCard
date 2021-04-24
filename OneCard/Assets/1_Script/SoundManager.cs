using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // AudioSource�� Ű��(Key, Value)�� �޾� �����ϴ� ����
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

        // SoundManager��� ���� ������Ʈ�� ����
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

    // Ű���� ����ϴ� �Լ�
    public void Regist(int iInAudioKey, AudioClip oInAudioClip)
    {
        Debug.Assert(oInAudioClip != null, "Invalid AudioClip! AudioKey= " + iInAudioKey.ToString());

        // oAudioClipsMap�� iInAudioKey�� �����Ѵٸ� ������� �ʰ� return
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

        // �ݺ��Ͽ� ����� AudioSource��� �ݺ����
        if (IsLoop)
        {
            Debug.Assert(oAS_Loop != null, "AudioSource is null!");
            oAS_Loop.Stop();
            oAS_Loop.clip = oAudioClipsMap[iInAudioKey];
            oAS_Loop.Play();
        }
        // �ѹ��� ����� AudioSource��� �ѹ��� ���
        else
        {
            Debug.Assert(oAS_Once != null, "AudioSource is null!");
            oAS_Once.PlayOneShot(oAudioClipsMap[iInAudioKey]);
        }
    }
}
