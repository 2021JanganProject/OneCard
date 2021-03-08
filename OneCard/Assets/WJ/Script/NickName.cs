using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NickName : MonoBehaviour
{
    public InputField inputName;


    public void Save()
    {
        
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Play");
    }

}
