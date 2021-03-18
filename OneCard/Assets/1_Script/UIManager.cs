using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject[] currentAttackCountItemArr;

    private Text LeftTime;
    private GameObject ArrowRightObject;
    private GameObject ArrowLeftObject;
    [SerializeField] private GameObject changeShapeUI;

    public GameObject ChangeShapeUI { get => changeShapeUI; set => changeShapeUI = value; }

    private void SetCurrentAttackCountItemp(int num)
    {

    }

    private void SetLeftTime(int playerNumber, int currentTime)
    {

    }
}
