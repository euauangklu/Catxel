using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    public static GameObject MiniGameButton;
    [SerializeField]private float ValueNum;
    [SerializeField] private float Timer;
    [SerializeField] private List<GameObject> DeactiveList;
    [SerializeField] private List<GameObject> HeartList;
    [SerializeField] private Slider Slider;
    private Animator StickAnimator;
    private bool InPlay;
    private float HeartValue;
    public bool Done;
    void Start()
    {
        MiniGameLoad();
    }

    void Update()
    {
        GameObject stickObj = GameObject.FindWithTag("Stick"); 
        StickAnimator = stickObj.GetComponent<Animator>();
        if (Slider.value >= Slider.maxValue)
        {
            HeartValue += 1;
            Slider.value = 0;
        }

        if (HeartValue == 1)
        {
            HeartList[0].SetActive(true);
        }
        if (HeartValue == 2)
        {
            HeartList[1].SetActive(true);
        }
        if (HeartValue == 3)
        {
            HeartList[2].SetActive(true);
            ResetMiniGame();
            Done = true;
        }
        SetActiveList();
    }

    private void MiniGameLoad()
    {
        if (MiniGameButton == null)
        {
            MiniGameButton = gameObject;
            gameObject.SetActive(false);
        }
    }

    private void SetActiveList()
    {
        for (int i = 0; i < DeactiveList.Count; i++)
        {
            if (!Done)
            {
                DeactiveList[i].SetActive(false);
            }

            else if (Done)
            {
                DeactiveList[i].SetActive(true);
            }
        }
    }

    public void AddValue()
    {
        Slider.value += ValueNum;
        StickAnimator.SetTrigger("Play");
    }

    private void ResetMiniGame()
    {
        HeartValue = 0;
        Slider.value = 0;
        gameObject.SetActive(false);
        for (int i = 0; i < HeartList.Count; i++)
        {
            HeartList[i].SetActive(false);
        }
    }
}
