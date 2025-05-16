using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private int Money;
    
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float minJumpDistance = 0.1f;
    [SerializeField] private float maxJumpDistance = 1f;
    [SerializeField] private float jumpDuration = 1f;
    private Vector3 startPos;
    private Vector3 endPos;
    private float timer = 0f;
    private bool isJumping = true;
    void Start()
    {
        startPos = transform.position;
        float direction = Random.value < 0.5f ? -1f : 1f;
        float randomDistance = Random.Range(minJumpDistance, maxJumpDistance);
        endPos = startPos + new Vector3(randomDistance  * direction, 0f, 0f);
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(endPos);
        if (screenPoint.x < 0f || screenPoint.x > 1f)
        {
            direction *= -1f;
            endPos = startPos + new Vector3(randomDistance  * direction, 0f, 0f);
        }
    }

    void Update()
    {
        if (isJumping)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / jumpDuration);
            float heightOffset = 4 * jumpHeight * t * (1 - t);
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            pos.y += heightOffset;
            transform.position = pos;

            if (t >= 1f)
            {
                isJumping = false;
            }
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
                    MoneyManager.Instance.AddMoney(Money);
                    Destroy(gameObject);
                }
            }
        }
    }
}
