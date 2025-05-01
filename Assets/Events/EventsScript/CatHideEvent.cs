using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Hide Event")]
public class CatHideEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;
    
    private GameObject MainCat;
    
    private GameObject HidePoint;
    
    private RandomEventManager manager;
    
    public override void TriggerEvent(RandomEventManager mgr)
    {
        MainCat = GameObject.FindWithTag("MainCat");
        GameObject HidePoint = GameObject.FindWithTag("HidePoint");
        GameObject obj = Instantiate(_gameObject,HidePoint.transform.position, Quaternion.identity);
        CatHide cat = obj.GetComponent<CatHide>();
        MainCat.SetActive(false);
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
