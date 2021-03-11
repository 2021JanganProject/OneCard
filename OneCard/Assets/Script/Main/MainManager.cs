using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField] Profile Profile;
    // Start is called before the first frame update
    void Start()
    {
        Profile.setProfile(DataManager.instance.CurrentPlayerInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
