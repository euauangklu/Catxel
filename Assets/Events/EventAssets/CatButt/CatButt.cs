using UnityEngine;
using System.Collections;

public class CatButt : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float TouchTime;
    private float touchTimeCount;
    private Vector3 originalPos;
    private bool isShaking = false;
    void Start()
    {
        originalPos = transform.position;
    }
    void Update()
    {
        if (touchTimeCount >= TouchTime)
        {
            Destroy(this.gameObject);
            Time.timeScale = 1;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D col = Physics2D.OverlapPoint(touchPos);
                if (col != null && col.gameObject == this.gameObject && !isShaking)
                {
                    StartCoroutine(Shake(shakeDuration, shakeMagnitude)); 
                    touchTimeCount++;
                }
            }
        }
        
    }
    private IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0f);

            transform.position = originalPos + randomOffset;
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
        isShaking = false;
    }
}
