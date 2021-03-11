using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{

    [SerializeField] float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForSec());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("02_Login");
    }
}
