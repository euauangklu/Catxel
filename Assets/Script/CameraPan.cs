using UnityEngine;

public class CameraPan : MonoBehaviour
{
    private Vector3 lastPanPosition;
    private Vector3 targetPosition;

    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    [SerializeField] private float panSmoothSpeed = 10f;

    [SerializeField] private RandomEventManager RandomEventManager;

    [SerializeField] private MicrophoneManager MicrophoneManager;
    
    [SerializeField] private DragNDrop DragNDrop;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (RandomEventManager.isInCatButtEvent)
        {
            transform.position = new Vector3(0,0,-10);
        }
        
        if (MicrophoneManager.pressing || DragNDrop.isDragging)
        {
            return;
        }
        
        if (!MicrophoneManager.pressing && !RandomEventManager.isInCatButtEvent && !DragNDrop.isDragging)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 currentPanPosition = GetWorldPos(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = currentPanPosition;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 diff = lastPanPosition - currentPanPosition;
                    targetPosition += new Vector3(diff.x, 0f, 0f);
                    lastPanPosition = currentPanPosition;
                }
            }

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = transform.position.y;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, panSmoothSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetWorldPos(Vector3 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        world.z = 0f;
        return world;
    }
}
