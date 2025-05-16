using UnityEngine;

public class CameraPan : MonoBehaviour
{
    private Vector3 lastPanPosition;
    private bool isPanning;

    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    void Update()
    {
        Vector3 currentPanPosition;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = GetWorldPos(Input.mousePosition);
            isPanning = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            currentPanPosition = GetWorldPos(Input.mousePosition);
            PanCamera(currentPanPosition);
        }
#endif


        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            currentPanPosition = GetWorldPos(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = currentPanPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                PanCamera(currentPanPosition);
            }
        }

        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, minX, maxX);
        transform.position = clamped;
    }

    private Vector3 GetWorldPos(Vector3 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        world.z = 0f;
        return world;
    }

    private void PanCamera(Vector3 currentPanPosition)
    {
        Vector3 diff = lastPanPosition - currentPanPosition;
        transform.position += new Vector3(diff.x, 0f, 0f);
        lastPanPosition = currentPanPosition;
    }
}
