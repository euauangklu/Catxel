using System;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private string itemID;
    public string ItemID => itemID;
    public int itemPrice = 100;
    [SerializeField] private GameObject SoldOut;
    [SerializeField] private GameObject Button;
    private bool AlreadyBuy;

    private void Awake()
    {
    }
    
    public void LoadBuyState()
    {
        if (PlayerPrefs.GetInt("Buy_" + itemID, 0) == 1)
        {
            AlreadyBuy = true;
            gameObject.SetActive(true);
            SoldOut.SetActive(true);
            Button.SetActive(false);
        }
        else
        {
            AlreadyBuy = false;
            gameObject.SetActive(false);
            SoldOut.SetActive(false);
            Button.SetActive(true);
        }
    }

    public void BuyItem()
    {
        if (AlreadyBuy) return;
        bool success = MoneyManager.Instance.SpendMoney(itemPrice , AlreadyBuy);
        if (success)
        {
            PlayerPrefs.SetInt("Buy_" + itemID,1);
            PlayerPrefs.Save();
            LoadBuyState();
        }
    }
}
