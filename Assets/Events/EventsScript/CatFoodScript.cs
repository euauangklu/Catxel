using Unity.VisualScripting;
using UnityEngine;
using System;

public class CatFoodScript : MonoBehaviour
{
    public int CatFoodNum;
    [SerializeField] private GameObject CatFoodIcon;
    [SerializeField] private GameObject CatFoodAnimationObj;
    public RandomEventManager RandomEventManager;
    [SerializeField] private float SetHours;
    private bool PlayAnimation;
    private bool Done;
    private bool Shaking;
    private int ShakingTime;
    private float Timer;
    private float Cooldown;
    private bool StartCooldown;
    void Start()
    {
        LoadSavedCatFood();
        LoadCatFoodByRealTime();
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.SetInt("CatFoodNum", CatFoodNum);
        PlayerPrefs.Save();
    }

    void LoadSavedCatFood()
    {
        if (PlayerPrefs.HasKey("CatFoodNum"))
        {
            CatFoodNum = PlayerPrefs.GetInt("CatFoodNum");
        }
    }
    
    void LoadCatFoodByRealTime()
    {
        if (PlayerPrefs.HasKey("LastPlayTime"))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("LastPlayTime"));
            DateTime lastTime = DateTime.FromBinary(temp);
            TimeSpan timeAway = DateTime.Now - lastTime;

            int unitsLost  = Mathf.FloorToInt((float)timeAway.TotalHours / SetHours);
            CatFoodNum -= unitsLost ;

            if (CatFoodNum < 0)
            {
                CatFoodNum = 0;
            }
        }
    }

    void Update()
    {
        if (!RandomEventManager.EventRandom)
        {
            if (StartCooldown && CatFoodNum == 0)
            {
                Cooldown -= Time.deltaTime;
            }

            if (CatFoodNum == 0 && Cooldown <= 0)
            {
                if (!PlayAnimation)
                {
                    CatFoodIcon.SetActive(true);
                    Cooldown = 0;
                    StartCooldown = false;
                }

                if (PlayAnimation && !Done)
                {
                    PlayAnimationCatFood();
                }

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        Collider2D col = Physics2D.OverlapPoint(touchPos);
                        if (col != null && col.gameObject == this.gameObject)
                        {
                            CatFoodAnimationObj.SetActive(true);
                            CatFoodIcon.SetActive(false);
                            PlayAnimation = true;
                        }
                    }
                }

                if (Done)
                {
                    CatFoodNum = 3;
                    CatFoodAnimationObj.SetActive(false);
                }
            }

            if (CatFoodNum == 3)
            {
                ResetCatFood();
            }
        }
    }

    private void PlayAnimationCatFood()
    {
        if (CatFoodAnimationObj.transform.eulerAngles.z <= 100f && !Done)
        {
            CatFoodAnimationObj.transform.Rotate(0,0,60 * Time.deltaTime);
        }
        else if (CatFoodAnimationObj.transform.eulerAngles.z > 100f && !Done)
        {
            if (!Shaking && ShakingTime < 3)
            {
                Timer += Time.deltaTime;
                CatFoodAnimationObj.transform.position += new Vector3(0,0.01f);
                if (Timer >= 0.15f)
                {
                    Shaking = true;
                    Timer = 0;
                }
            }
            else if (Shaking && ShakingTime < 3)
            {
                Timer += Time.deltaTime;
                CatFoodAnimationObj.transform.position -= new Vector3(0,0.01f);
                if (Timer >= 0.15f)
                {
                    ShakingTime += 1;
                    Shaking = false;
                    Timer = 0;
                }
            }

            if (ShakingTime >= 3)
            {
                Done = true;
            }
        }
    }

    private void ResetCatFood()
    {
        PlayAnimation = false;
        Done = false;
        Cooldown = 3;
        StartCooldown = true;
        ShakingTime = 0;
        CatFoodAnimationObj.transform.rotation = Quaternion.identity;
    }
}
