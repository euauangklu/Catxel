using UnityEngine;

public class CameraPan : MonoBehaviour
{
    private Vector3 lastPanPosition;
    
    private Vector3 velocity;

    [SerializeField] private float minX = -5f;
    
    [SerializeField] private float maxX = 5f;
    
    [SerializeField] private float panSpeedMultiplier = 1.0f;
    
    [SerializeField] private float friction = 5f;

    [SerializeField] private RandomEventManager RandomEventManager;

    [SerializeField] private MicrophoneManager MicrophoneManager;
    
    private DragNDrop DragNDrop;
    
    private bool isTouchingDraggable = false;

    void Update()
    {
        if (RandomEventManager != null && RandomEventManager.isInCatButtEvent)
        {
            transform.position = new Vector3(0, 0, -10);
            return;
        }

        if (MicrophoneManager != null && MicrophoneManager.pressing) return;
        if (RandomEventManager != null && RandomEventManager.isInCatStickEvent) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                Collider2D col = Physics2D.OverlapPoint(touchPos);
                if (col != null)
                {
                    DragNDrop = col.GetComponent<DragNDrop>();
                    isTouchingDraggable = DragNDrop != null;
                }

                lastPanPosition = GetWorldPos(touch.position);
                velocity = Vector3.zero;
            }
            else if (touch.phase == TouchPhase.Moved && !isTouchingDraggable)
            {
                Vector3 currentPanPosition = GetWorldPos(touch.position);
                Vector3 diff = lastPanPosition - currentPanPosition;

                velocity = diff * panSpeedMultiplier / Time.deltaTime;
                lastPanPosition = currentPanPosition;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouchingDraggable = false;
            }
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, friction * Time.deltaTime);
        }

        float newX = transform.position.x + velocity.x * Time.deltaTime;
        newX = Mathf.Clamp(newX, minX, maxX);
    
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private Vector3 GetWorldPos(Vector3 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        world.z = 0f;
        return world;
    }
}
