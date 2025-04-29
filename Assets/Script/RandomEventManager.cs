using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    
    public bool EventRandom;
    
    public GameEvents[] gameEvents;

    public float EventTimer;

    private float Timer;
    void Update()
    {
        if (Timer < EventTimer)
        {
            Timer += Time.deltaTime;
        }
        if (!EventRandom && Timer >= EventTimer)
        {
            TriggerRandomEvent();
        }
    }
    public void TriggerRandomEvent()
    {
        var e = gameEvents[Random.Range(0, gameEvents.Length)];
        e.TriggerEvent(this);
        EventRandom = true;
    }

    public void ResetRandomEvent()
    {
        Player.SetActive(true);
        EventRandom = false;
        Timer = 0;
    }
    
}
