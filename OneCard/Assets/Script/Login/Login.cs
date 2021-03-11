using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{

    [SerializeField] GameObject loginFiled;
    [SerializeField] Text loginText;

    
    // Start is called before the first frame update
    void Start()
    {
        loginFiled.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoginLoadingOver()
    {
        CheckLoginActiveLoginField(LoginManager.instance.fireBaseManager.authManager.IsLogined());
    }
    void CheckLoginActiveLoginField(bool isLogined)
    {
        

    }

    public void Signedin()
    {
        loginFiled.SetActive(false);
        loginText.text = "환영합니다";
        StartCoroutine(WaitAndNextScene(2));
    }
    public void SignedOut()
    {
        loginFiled.SetActive(true);
        loginText.text = "";
    }


    IEnumerator WaitAndNextScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        SceneManager.LoadScene("03_Main");
    }

}
