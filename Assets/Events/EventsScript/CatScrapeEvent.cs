using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Scrape Event")]
public class CatScrapeEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;
    
    [SerializeField] private string requiredItemID;
    
    private GameObject MainCat;
    
    private RandomEventManager manager;
    
    public override void TriggerEvent(RandomEventManager mgr)
    {
        manager = mgr;
        
        if (!BuyItemManager.Instance.IsItemBought(requiredItemID))
        {
            OnEventDone();
        }

        else if (BuyItemManager.Instance.IsItemBought(requiredItemID))
        {
            MainCat = GameObject.FindWithTag("MainCat");
            GameObject obj = Instantiate(_gameObject,MainCat.transform.position, Quaternion.identity);
            CatScrape cat = obj.GetComponent<CatScrape>();
            MainCat.SetActive(false);
            if (cat != null)
            {
                cat.eventSource = this;
            }
        }
    }
    
    public void OnEventDone()
    {
        if (manager != null)
        {
            if (BuyItemManager.Instance.IsItemBought(requiredItemID))
            {
                manager.DoneEvent = true;
            }
            manager.ResetRandomEvent();
        }
    }
}
