using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public bool isTest = false;
    public bool isHojun = false;
    [SerializeField] float waitTime;
    // Start is called before the first frame update
    [SerializeField] DataManager dataManagerForDebug;
    void Start()
    {
        if (isHojun == true)
        {
            Instantiate(dataManagerForDebug).InitPlayerInfoForNoUseFirebaseDebug();
            Screen.SetResolution(640, 360, false);
            StartCoroutine(WaitForSecTest("Game_Test"));
            return;
        }
        if (isTest == true)
        {
            Instantiate(dataManagerForDebug).InitPlayerInfoForNoUseFirebaseDebug();
            StartCoroutine(WaitForSecTest("03_Main"));

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
    IEnumerator WaitForSecTest(string name)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(name);
        
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("02_Login");
    }
}
