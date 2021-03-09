using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class attackCounter : MonoBehaviour
{
    int ACount = 0;
   
    [SerializeField]
    private GameObject[] CountImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetAcount();
    }
    void SetAcount()
    {
        if (ACount > 0 && ACount <= CountImage.Length)
        {
            for (int i = 0; i < ACount; i++)
            {
                CountImage[i].SetActive(true);
            }
        }
    }

    public void Acount2Up()
    {
        ACount += 2;
    }
    public void Acount3Up()
    {
        ACount += 3;
    }
}
