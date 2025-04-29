using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Butt Event")]
public class CatButtEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;
    
    private GameObject MainCat;
    
    private RandomEventManager manager;

    public override void TriggerEvent(RandomEventManager mgr)
    { 
        MainCat = GameObject.FindWithTag("MainCat");
        GameObject obj = Instantiate(_gameObject, new Vector3(0, -1.5f, 0), Quaternion.identity);
        CatButt cat = obj.GetComponent<CatButt>();
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
