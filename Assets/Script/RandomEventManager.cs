using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Rem;
    
    [SerializeField] private GameObject Cat;
    
    public bool EventRandom;
    
    public GameEvents[] gameEvents;

    public float EventTimer;

    private float Timer;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("LastPlayTime"))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("LastPlayTime"));
            DateTime lastTime = DateTime.FromBinary(temp);
            TimeSpan timeAway = DateTime.Now - lastTime;

            Timer += (float)timeAway.TotalSeconds;
        }
    }

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
        MainCatManager.MainCat.GetComponent<MainCatManager>().CatEXP += MainCatManager.MainCat.GetComponent<MainCatManager>().EXPPerEvent;
        PlayerPrefs.SetInt("CatEXP", MainCatManager.MainCat.GetComponent<MainCatManager>().CatEXP);
        PlayerPrefs.Save();
        MoneyManager.Instance.SpawningPrefab(5);
        Cat.SetActive(true);
        EventRandom = false;
        Timer = 0;
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }
}
