using System.Timers;
using UnityEngine;

public class CatPlayToy : MonoBehaviour
{
    [SerializeField] private GameObject PlayIcon;
    [SerializeField] private GameObject CatToy;
    [SerializeField] private float PlayTimer;
    [SerializeField] private float Dropspeed;
    private float timer;
    private bool StartEvent;
    public CatPlayToyEvent eventSource;
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;
    void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!StartEvent)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    Collider2D col = Physics2D.OverlapPoint(touchPos);
                    if (col != null && col.gameObject == this.gameObject)
                    {
                        PlayIcon.SetActive(false);
                        CatToy.SetActive(true);
                        StartEvent = true;
                    }
                }
            }
        }
        else if (StartEvent)
        {
            if (CatToy.gameObject.transform.localPosition.y > 0.1f)
            {
                CatToy.gameObject.transform.localPosition += Vector3.down * Time.deltaTime * Dropspeed;
            }
            else if (CatToy.gameObject.transform.localPosition.y <= 0.1f)
            {
                SpriteRenderer.flipX = true;
                Animator.SetBool("Playing",true);
                timer += Time.deltaTime;
                if (PlayTimer < timer)
                {
                    eventSource.OnEventDone();
                    Destroy(this.gameObject);
                    timer = 0;
                    StartEvent = false;
                    Animator.SetBool("Playing",false);
                }
            }
        }
        
    }
}
