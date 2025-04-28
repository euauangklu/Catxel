using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Climb Curtain Event")]
public class CatClimbCurtainEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;
    
    private GameObject ClimbPoint;
    
    private RandomEventManager manager;
    public override void TriggerEvent(RandomEventManager mgr)
    {
        GameObject ClimbPoint = GameObject.FindWithTag("ClimbPoint");
        GameObject obj = Instantiate(_gameObject,ClimbPoint.transform.position, Quaternion.identity);
        CatClimbCurtain cat = obj.GetComponent<CatClimbCurtain>();
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
