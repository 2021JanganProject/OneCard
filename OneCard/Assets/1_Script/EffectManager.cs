using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;

    public static EffectManager Instance 
    { 
        get
        {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<EffectManager>();
            }
            return m_Instance;
        }
    }

    public ParticleSystem[] Effects;
    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }
    public void PlayEffect(int EffectNum, Vector3 pos, Vector3 normal, Transform parent = null)
    {
        var targetPrefab = Effects[EffectNum];
        var effect = Instantiate(targetPrefab, pos, Quaternion.LookRotation(normal));

        if(parent != null)
        {
            effect.transform.SetParent(parent);
        }
        effect.Play();
        Destroy(effect.gameObject, 3.0f);
    }

    #region == BtnEvts ==  
    public void BtnEvt_Effect()
    {
        EffectManager.Instance.PlayEffect(5, this.transform.position, this.transform.position);
    }
    #endregion
}
