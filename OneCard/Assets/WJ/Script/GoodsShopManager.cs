using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsShopManager : MonoBehaviour
{
    private const int GOLD_CONTENT_COUNT = 6;
    private const int CRYSTAL_CONTENT_COUNT = 5;
    private const int HEART_CONTENT_COUNT = 5;
    [SerializeField] private GameObject goodsShopObj;
    [SerializeField] private GameObject popUpObj;

    
    private static GoodsShopManager instance = null;
    public static GoodsShopManager Instance { get => instance; }

    private GameObject gold;
    private GameObject crystal;
    private GameObject heart;
    private GameObject currentCategory;

    [SerializeField] private Content[] contentsGold;
    [SerializeField] private Content[] contentsCrystal;
    [SerializeField] private Content[] contentsHeart;
    [SerializeField] private List<Goods> goods = new List<Goods>();

    private Content currentContent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gold = goodsShopObj.transform.Find("ContentArea").Find("Content-Gold").gameObject;
        crystal = goodsShopObj.transform.Find("ContentArea").Find("Content-Crystal").gameObject;
        heart = goodsShopObj.transform.Find("ContentArea").Find("Content-Heart").gameObject;

        
        contentsGold = gold.transform.GetComponentsInChildren<Content>();       
        contentsCrystal = crystal.transform.GetComponentsInChildren<Content>();
        contentsHeart = heart.transform.GetComponentsInChildren<Content>();

        int j = 0;
        for (int i = 0; i < goods.Count; i++)
        {
            if (i <= GOLD_CONTENT_COUNT - 1)
            {
                contentsGold[i].Goods = goods[i];
            }
            else if (i >= GOLD_CONTENT_COUNT)
            {               
                contentsHeart[j].Goods = goods[i];
                j++;
            }
        }
        currentCategory = gold;
    }
    #region -BtnEvt-
    public void BtnEvt_PopUpGoodsShop()
    {
        goodsShopObj.SetActive(true);
    }
    public void BtnEvt_ExitGoodsShop()
    {
        goodsShopObj.SetActive(false);
    }
    public void BtnEvt_ShowCategoryGold()
    {
        if (goodsShopObj.activeSelf == false)
        {
            goodsShopObj.SetActive(true);
        }
        ShowCategory(gold);
    }
    public void BtnEvt_ShowCategoryCrystal()
    {
        if (goodsShopObj.activeSelf == false)
        {
            goodsShopObj.SetActive(true);
        }
        ShowCategory(crystal);
    }
    public void BtnEvt_ShowCategoryHeart()
    {
        if (goodsShopObj.activeSelf == false)
        {
            goodsShopObj.SetActive(true);
        }
        ShowCategory(heart);
    }
    public void BtnEvt_ClosePopUp()
    {
        popUpObj.SetActive(false);
    }
#endregion

    private void ShowCategory(GameObject category)
    {
        if (currentCategory == category) return;
        currentCategory.SetActive(false);
        category.SetActive(true);
        currentCategory = category;
    }
    public void ShowPopUpOfTheContent(Content content)
    {
        popUpObj.SetActive(true);
        var image = popUpObj.transform.GetChild(0).Find("ContentImage").GetComponent<Image>();
        image.sprite = content.ContentImage.sprite;
        var txt = popUpObj.transform.GetChild(0).Find("PriceArea").GetChild(0).GetComponent<Text>();
        txt.text = content.PriceTxt.text;
        currentContent = content;
    }
 
    public void PurchaseGoods(Content content)
    {
        //ÄÁÅÙÃ÷ ±¸¸Å ·ÎÁ÷
    }
}
