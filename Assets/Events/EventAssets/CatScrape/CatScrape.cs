using Unity.VisualScripting;
using UnityEngine;

public class CatScrape : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private DragNDrop DragNDrop;
    [SerializeField] private Vector3 FloorPos;
    [SerializeField] private float SetTime;
    [SerializeField] private float fallSpeed = 0.5f;
    private Animator Animator;
    private bool AlreadyDrag;
    private bool AlreadyDrop;
    private bool ReadyEvent;
    private GameObject ScrapePoint;
    public CatScrapeEvent eventSource;
    private float Timer;
    private bool WaitForScale;
    

    void Start()
    {
        Animator = _gameObject.GetComponent<Animator>();
        ScrapePoint = GameObject.FindWithTag("ScrapePoint");
    }

    void Update()
    {
        var MainCat = MainCatManager.MainCat;
        if (Vector2.Distance(transform.position, ScrapePoint.transform.position) >= 0.1f && !ReadyEvent)
        {
            if (transform.position.x < ScrapePoint.transform.position.x)
            {
                transform.localScale= new Vector2(1, 1);
            }
            else if (transform.position.x > ScrapePoint.transform.position.x)
            {
                transform.localScale= new Vector2(-1, 1);
            }
            transform.position = Vector2.MoveTowards(transform.position, ScrapePoint.transform.position,  0.48f * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, ScrapePoint.transform.position) < 0.1f && !ReadyEvent)
        {
            Animator.SetBool("Scrape",true);
            ReadyEvent = true;
        }

        if (ReadyEvent)
        {
            if (!AlreadyDrop)
            {
                if (Timer <= 0.15f && !WaitForScale)
                {
                    Timer += Time.deltaTime;
                }
                if (Timer > 0.15f)
                {
                    transform.localScale= new Vector2(1, 1);
                    WaitForScale = true;
                    Timer = 0;
                }
                if (DragNDrop.isDragging)
                {
                    Animator.SetBool("IsDragging",true);
                    AlreadyDrag = true;
                }
                else if (!DragNDrop.isDragging)
                {
                    if (AlreadyDrag)
                    {
                        AlreadyDrop = true;
                        Animator.SetBool("IsDragging",false);
                        Animator.SetBool("Scrape",false);
                    }
                }
            }
            if (_gameObject.transform.position.y >= FloorPos.y && AlreadyDrop)
            {
                _gameObject.transform.position -= new Vector3(0, fallSpeed * Time.deltaTime,0);
            }
            else if (_gameObject.transform.position.y < FloorPos.y && AlreadyDrop)
            {
                Animator.SetBool("Idle",true);
                if (Timer <= SetTime)
                {
                    Timer += Time.deltaTime;
                }
                else if (Timer > SetTime)
                {
                    AlreadyDrop = false;
                    AlreadyDrag = false;
                    MainCat.transform.position = this.transform.position;
                    eventSource.OnEventDone();
                    Destroy(this.gameObject);
                }
            } 
        }
    }
}
