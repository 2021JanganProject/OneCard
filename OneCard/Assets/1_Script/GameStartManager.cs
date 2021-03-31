using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public bool isTest = false;
    [SerializeField] float waitTime;
    // Start is called before the first frame update
    [SerializeField] DataManager dataManagerForDebug;
    void Start()
    {
        
        if (isTest == true)
        {
            Instantiate(dataManagerForDebug).InitPlayerInfoForNoUseFirebaseDebug();
            StartCoroutine(WaitForSecTest());

            Screen.SetResolution(640,360, false);
        }
        else
        {
            StartCoroutine(WaitForSec());
            Screen.SetResolution(1920, 1080, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForSecTest()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("03_Main");
        
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("02_Login");
    }
}
