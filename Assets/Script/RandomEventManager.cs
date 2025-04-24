using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public GameEvents[] gameEvents;
    
    public bool EventRandom;

    void Update()
    {
        if (!EventRandom && Input.GetKey(KeyCode.E))
        {
            TriggerRandomEvent();
        }
    }
    public void TriggerRandomEvent()
    {
        var e = gameEvents[Random.Range(0, gameEvents.Length)];
        e.TriggerEvent();
        EventRandom = true;
        Time.timeScale = 0;
    }

    public void ResetRandomEvent()
    {
        
    }
    
}
