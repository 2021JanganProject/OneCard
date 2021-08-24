using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    [SerializeField] private Goods goods;
    private Category category;
    private Text priceTxt;
    [SerializeField] private Image contentImage;

    [SerializeField] private int amount;
    [SerializeField] private int price;

    public Goods Goods { set => goods = value; }
    public int Amount { get => amount; }
    public int Price { get => price;}
    public Image ContentImage { get => contentImage; }
    public Text PriceTxt { get => priceTxt; }
    // Start is called before the first frame update
    void Start()
    {
        priceTxt = GetComponentInChildren<Text>();
        contentImage = GetComponent<Image>();
        if (goods != null)
        {
            category = goods.Category;
            amount = goods.Amount;
            price = goods.Price;
            priceTxt.text = price == 0? "¹«·á" : $"{price.ToString()}¿ø";
            if(goods.Image != null)
            {
                contentImage.sprite = goods.Image;
                contentImage.color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnEvt_ClickContent()
    {
        if (goods != null)
        {
            GoodsShopManager.Instance.ShowPopUpOfTheContent(this);
        }
    }
}
