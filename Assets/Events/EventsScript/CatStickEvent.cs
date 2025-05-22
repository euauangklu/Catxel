using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Stick Event")]
public class CatStickEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;

    private GameObject MainCat;

    private RandomEventManager manager;

    public override void TriggerEvent(RandomEventManager mgr)
    {
        MainCat = GameObject.FindWithTag("MainCat");
        GameObject obj = Instantiate(_gameObject, MainCat.transform.position, Quaternion.identity);
        CatStick cat = obj.GetComponent<CatStick>();
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
            manager.isInCatStickEvent = false;
            manager.DoneEvent = true;
            manager.ResetRandomEvent();
        }
    }
}