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
    [SerializeField] private GameObject Coins;
    [SerializeField] private GameObject MainCat;
    [SerializeField] private float DelaySpawnTimer;
    private float timer; 
    private int spawnCount = 0;
    private int maxSpawn = 0;
    private bool isSpawning = false;
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
    }

    void Update()
    {
        //SpawnCoin
        if (isSpawning)
        {
            timer += Time.deltaTime;
        }
        if (timer >= DelaySpawnTimer)
        {
            Instantiate(Coins, MainCat.transform.position, Quaternion.identity);
            timer = 0f;
            spawnCount++;

            if (spawnCount >= maxSpawn)
            {
                isSpawning = false; 
            }
        }
        //MoneyText
        moneyText.text = "" + Money;
        //OpenShopList
        if (ShopOpen)
        {
            ShopList.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!ShopOpen)
        {
            ShopList.SetActive(false);
            Time.timeScale = 1;
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

    public void SpawningPrefab(int SpawnTime)
    {
        spawnCount = 0;
        maxSpawn = SpawnTime;
        timer = 0f;
        isSpawning = true;
    }

    
}
