using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Scrape Event")]
public class CatScrapeEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;
    
    private GameObject ScrapePoint;
    
    private RandomEventManager manager;

    public override void TriggerEvent(RandomEventManager mgr)
    {
        GameObject ScrapePoint = GameObject.FindWithTag("ScrapePoint");
        GameObject obj = Instantiate(_gameObject,ScrapePoint.transform.position, Quaternion.identity);
        CatScrape cat = obj.GetComponent<CatScrape>();
        if (cat != null)
        {
            cat.eventSource = this;
        }
        
        manager = mgr;
    }
    
    public void OnEventDone()
    {
        if (manager != null)
        {
            manager.ResetRandomEvent();
        }
    }
}
