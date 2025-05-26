using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [Header("Pan Settings")]
    [SerializeField] private float minX = -1f;
    [SerializeField] private float maxX = 1f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float friction = 5f; // ยิ่งมาก ยิ่งหยุดเร็ว

    [Header("Manager")]
    [SerializeField] private RandomEventManager RandomEventManager;
    [SerializeField] private MicrophoneManager MicrophoneManager;

    private DragNDrop DragNDrop;

    private float targetX;
    private float velocityX = 0f;
    private bool isTouchingDraggable = false;

    private Vector2 lastTouchScreenPos;
    private float lastDeltaX = 0f;

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        if (RandomEventManager != null && RandomEventManager.isInCatButtEvent)
        {
            transform.position = new Vector3(0, 0, -10);
            return;
        }
        if ((MicrophoneManager != null && MicrophoneManager.pressing) || (RandomEventManager != null && RandomEventManager.isInCatStickEvent))
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchScreenPos = touch.position;
                velocityX = 0f;

                Vector3 touchWorldPos = GetWorldPos(touch.position);
                Collider2D col = Physics2D.OverlapPoint(touchWorldPos);
                if (col != null)
                {
                    DragNDrop = col.GetComponent<DragNDrop>();
                    isTouchingDraggable = DragNDrop != null;
                }
            }
            else if (touch.phase == TouchPhase.Moved && !isTouchingDraggable)
            {
                float screenDelta = touch.position.x - lastTouchScreenPos.x;
                if (Mathf.Abs(screenDelta) < 5f) return;
                float percentMoved = screenDelta / Screen.width;
                float worldRange = maxX - minX;
                float moveDelta = -percentMoved * worldRange;
                targetX = Mathf.Clamp(transform.position.x + moveDelta, minX, maxX);
                lastDeltaX = moveDelta / Time.deltaTime;
                lastTouchScreenPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouchingDraggable = false;
                velocityX = lastDeltaX;
            }
        }
        else
        {
            if (Mathf.Abs(velocityX) > 0.01f)
            {
                targetX = Mathf.Clamp(transform.position.x + velocityX * Time.deltaTime, minX, maxX);
                velocityX = Mathf.Lerp(velocityX, 0f, friction * Time.deltaTime);
            }
        }

        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private Vector3 GetWorldPos(Vector3 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        world.z = 0f;
        return world;
    }
}
