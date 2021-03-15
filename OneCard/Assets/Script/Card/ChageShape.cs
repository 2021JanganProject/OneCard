using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChageShape : Card
{
    private UIManager uiManager;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    protected override void Put()
    {
        base.Put();
        ShowUIChangeShape();
    }

    protected override void Checking()
    {
        base.Checking();
    }

    private void ShowUIChangeShape()
    {
        uiManager.ChangeShapeUI.SetActive(true);
    }
}
