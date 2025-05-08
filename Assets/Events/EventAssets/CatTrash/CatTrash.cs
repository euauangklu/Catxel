using UnityEngine;
using System.Collections;

public class CatTrash : MonoBehaviour
{
    public CatTrashEvent eventSource;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D hit = Physics2D.OverlapPoint(touchPos);
                if (hit != null && hit.gameObject == gameObject)
                {
                    StartCoroutine(DestroyAndCheck());
                }
            }
        }
    }
    private IEnumerator DestroyAndCheck()
    {
        yield return new WaitForEndOfFrame();
        GameObject[] remaining = GameObject.FindGameObjectsWithTag("CatTrash");
        if (remaining.Length <= 1 && eventSource != null)
        {
            eventSource.OnEventDone();
        }
        MainCatManager.MainCat.GetComponent<MainCatManager>().CatEXP += MainCatManager.MainCat.GetComponent<MainCatManager>().EXPPerEvent;
        PlayerPrefs.SetInt("CatEXP", MainCatManager.MainCat.GetComponent<MainCatManager>().CatEXP);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
