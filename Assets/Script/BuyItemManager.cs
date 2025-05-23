using UnityEngine;

public class BuyItemManager : MonoBehaviour
{
    public static BuyItemManager Instance;
    
    [SerializeField] private ShopItem[] allItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        foreach (var item in allItems)
        {
            item.LoadBuyState();
        }
    }
    public void ReloadItems()
    {
        foreach (var item in allItems)
        {
            item.LoadBuyState();
        }
    }
    
    public bool IsItemBought(string itemID)
    {
        foreach (var item in allItems)
        {
            if (item.ItemID == itemID)
            {
                return item.AlreadyBuy;
            }
        }

        return false;
    }
}
