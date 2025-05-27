using UnityEngine;

public class CatHide : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SetTime;
    [SerializeField] private Vector2 WalkOutPos;
    private bool Siting;
    private bool WalkOut;
    private float Timer;
    private Animator Animator;
    private bool ReadyEvent;
    private GameObject HidePoint;
    public CatHideEvent eventSource;
    
    void Start()
    {
        Animator = _gameObject.GetComponent<Animator>();
        HidePoint = GameObject.FindWithTag("HidePoint");
    }
    void Update()
    {
        var MainCat = MainCatManager.MainCat;
        if (Vector2.Distance(transform.position, HidePoint.transform.position) >= 0.1f && !ReadyEvent)
        {
            if (transform.position.x < HidePoint.transform.position.x)
            {
                transform.localScale= new Vector2(1, 1);
            }
            else if (transform.position.x > HidePoint.transform.position.x)
            {
                transform.localScale= new Vector2(-1, 1);
            }
            transform.position = Vector2.MoveTowards(transform.position, HidePoint.transform.position, WalkSpeed * 0.48f * Time.deltaTime);
            Animator.SetBool("Walk",true);
        }

        if (Vector2.Distance(transform.position, HidePoint.transform.position) < 0.1f && !ReadyEvent)
        {
            Animator.SetBool("Walk",false);
            ReadyEvent = true;
        }

        if (!WalkOut && ReadyEvent)
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
                        WalkOut = true;
                        Animator.SetBool("Walk",true);
                        if (transform.position.x < WalkOutPos.x)
                        {
                            transform.localScale= new Vector2(1, 1);
                        }
                        else if (transform.position.x > WalkOutPos.x)
                        {
                            transform.localScale= new Vector2(-1, 1);
                        }
                    }
                }
            }
        }
        if (Vector2.Distance(transform.position, WalkOutPos) >= 0.1f && WalkOut)
        {
            transform.position = Vector2.MoveTowards(transform.position, WalkOutPos, WalkSpeed * 0.48f * Time.deltaTime);
        }
        if (Vector2.Distance(transform.position, WalkOutPos) < 0.1f && WalkOut)
        {
            Siting = true;
            Animator.SetBool("Walk",false);
        }

        if (Siting)
        {
            Timer += Time.deltaTime;
            if (SetTime <= Timer)
            {
                eventSource.OnEventDone();
                MainCat.transform.position = this.transform.position;
                Destroy(this.gameObject);
                Siting = false;
                WalkOut = false;
                ReadyEvent = false;
            }
        }
    }
}
