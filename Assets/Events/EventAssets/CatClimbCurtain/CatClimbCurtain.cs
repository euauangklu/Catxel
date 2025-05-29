using UnityEngine;

public class CatClimbCurtain : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private DragNDrop DragNDrop;
    [SerializeField] private Vector3 FloorPos;
    [SerializeField] private float SetTime;
    [SerializeField] private float fallSpeed = 0.5f;
    private Animator Animator;
    private bool AlreadyDrag;
    private bool AlreadyDrop;
    private float Timer;
    private bool ReadyEvent;
    private GameObject ClimbPoint;
    public CatClimbCurtainEvent eventSource;
    void Start()
    {
        Animator = _gameObject.GetComponent<Animator>();
        ClimbPoint = GameObject.FindWithTag("ClimbPoint");
        if (transform.position.x < ClimbPoint.transform.position.x) //TurnRight
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (transform.position.x > ClimbPoint.transform.position.x) //TurnLeft
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void Update()
    {
        var MainCat = MainCatManager.MainCat;
        if (!ReadyEvent)
        {
            if (Mathf.Abs(_gameObject.transform.position.x - ClimbPoint.transform.position.x) > 0.1f)
            {
                Vector3 currentPosition = _gameObject.transform.position;
                float targetX = Mathf.MoveTowards(currentPosition.x, ClimbPoint.transform.position.x,  0.48f * Time.deltaTime);
                float targetY = Mathf.MoveTowards(currentPosition.y, 0,  0.48f * Time.deltaTime);
                _gameObject.transform.position = new Vector3(targetX, targetY, currentPosition.z);
            }
            if (Mathf.Abs(_gameObject.transform.position.x - ClimbPoint.transform.position.x) <= 0.1f)
            {
                Animator.SetBool("Climb", true);
                Timer += Time.deltaTime;
                if (Timer >= 0.6f)
                {
                    _gameObject.transform.position = Vector2.MoveTowards(transform.position, ClimbPoint.transform.position,  3 * Time.deltaTime);
                }
            }
        
            if (Vector2.Distance(_gameObject.transform.position, ClimbPoint.transform.position) < 0.1f)
            {
                ReadyEvent = true;
                Timer = 0;
            }
        }
        if (ReadyEvent)
        {
            DragNDrop.EnableDrag = true;
            if (!AlreadyDrop)
            {
                if (DragNDrop.isDragging)
                {
                    Animator.SetBool("IsDragging",true);
                    AlreadyDrag = true;
                }
                else if (!DragNDrop.isDragging)
                {
                    Animator.SetBool("IsDragging",false);
                    if (AlreadyDrag)
                    {
                        Animator.SetBool("Climb", false);
                        AlreadyDrop = true;
                        DragNDrop.EnableDrag = false;
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
