using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugerManager : MonoBehaviour
{

    public static DebugerManager instance = null;
    public delegate void DebugDel();
    public DebugDel del_debugInputT;
    public DebugDel del_debugInputR;

    /** 상태 출력용 */
    public Text txtPrint;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Assert(false , "asd");
    }

    // Update is called once per frame
    void Update()
    {

        PressKeyLog();

    }

    public void PressKeyLog()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            del_debugInputT();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            del_debugInputR();
        }

    }
    public void Log(string logText)
    {
        txtPrint.text += (logText + "\n");
        Debug.Log(logText);
    }
    public void IsNull(object obj)
    {
        try
        {
            Log($"{obj.ToString()} 개체가 있습니다");
            
        }
        catch(Exception e)
        {
            
            Log($"{e.Message}");
        }
      
    }

}
