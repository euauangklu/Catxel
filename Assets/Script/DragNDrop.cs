using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    public bool isDragging = false;
    public bool EnableDrag;
    private Vector3 offset;

    void Update()
    {
        if (Input.touchCount > 0 && EnableDrag)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Collider2D hit = Physics2D.OverlapPoint(touchPos);
                    if (hit != null && hit.gameObject == gameObject)
                    {
                        isDragging = true;
                        offset = transform.position - (Vector3)touchPos;
                    }

                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                    {
                        Vector3 newPosition = touchPos + (Vector2)offset;

                        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
                        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

                        newPosition.x = Mathf.Clamp(newPosition.x, min.x, max.x);
                        newPosition.y = Mathf.Clamp(newPosition.y, min.y, max.y);

                        transform.position = newPosition;
                    }

                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }
}
