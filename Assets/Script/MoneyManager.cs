using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;
    private bool ShopOpen;
    public int Money { get; private set; }
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject ShopList;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Money = PlayerPrefs.GetInt("PlayerMoney", 0);
        AddMoney(500);
    }

    void Update()
    {
        moneyText.text = "" + Money;
        if (ShopOpen)
        {
            ShopList.SetActive(true);
        }
        else if (!ShopOpen)
        {
            ShopList.SetActive(false);
        }
    }

    private void OpenShop()
    {
        if (ShopOpen)
        {
            ShopOpen = false;
        }
        else if (!ShopOpen)
        {
            ShopOpen = true;
        }
    }
    
    public void AddMoney(int amount)
    {
        Money += amount;
        SaveMoney();
    }
    
    public bool SpendMoney(int amount , bool AlreadyBuy)
    {
        if (Money >= amount && !AlreadyBuy)
        {
            Money -= amount;
            SaveMoney();
            return true;
        }
        return false;
    }
    
    private void SaveMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", Money);
        PlayerPrefs.Save();
    }
    
}
