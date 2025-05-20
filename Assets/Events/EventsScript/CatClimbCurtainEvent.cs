using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Climb Curtain Event")]
public class CatClimbCurtainEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;
    
    private GameObject MainCat;
    
    private GameObject ClimbPoint;
    
    private RandomEventManager manager;
    public override void TriggerEvent(RandomEventManager mgr)
    {
        MainCat = GameObject.FindWithTag("MainCat");
        GameObject ClimbPoint = GameObject.FindWithTag("ClimbPoint");
        GameObject obj = Instantiate(_gameObject,ClimbPoint.transform.position, Quaternion.identity);
        CatClimbCurtain cat = obj.GetComponent<CatClimbCurtain>();
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
            manager.DoneEvent = true;
            manager.ResetRandomEvent();
        }
    }
}
