using System;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private string itemID;
    public string ItemID => itemID;
    public bool AlreadyBuy => alreadyBuy;
    public int itemPrice = 100;
    [SerializeField] private GameObject SoldOut;
    [SerializeField] private GameObject Button;
    [SerializeField] private TextMeshProUGUI TextPrice;
    private bool alreadyBuy;

    private void Awake()
    {
        TextPrice.text = itemPrice.ToString();
    }
    
    public void LoadBuyState()
    {
        if (PlayerPrefs.GetInt("Buy_" + itemID, 0) == 1)
        {
            alreadyBuy = true;
            gameObject.SetActive(true);
            SoldOut.SetActive(true);
            Button.SetActive(false);
        }
        else
        {
            alreadyBuy = false;
            gameObject.SetActive(false);
            SoldOut.SetActive(false);
            Button.SetActive(true);
        }
    }

    public void BuyItem()
    {
        if (alreadyBuy) return;
        bool success = MoneyManager.Instance.SpendMoney(itemPrice , alreadyBuy);
        if (success)
        {
            PlayerPrefs.SetInt("Buy_" + itemID,1);
            PlayerPrefs.Save();
            LoadBuyState();
        }
    }
}
