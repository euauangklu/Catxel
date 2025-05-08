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
    public CatClimbCurtainEvent eventSource;
    void Start()
    {
        Animator = _gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        var MainCat = MainCatManager.MainCat;
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
                    AlreadyDrop = true;
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
                MainCatManager.MainCat.GetComponent<MainCatManager>().CatEXP += MainCatManager.MainCat.GetComponent<MainCatManager>().EXPPerEvent;
                PlayerPrefs.SetInt("CatEXP", MainCatManager.MainCat.GetComponent<MainCatManager>().CatEXP);
                PlayerPrefs.Save();
                Destroy(this.gameObject);
            }
        }
    }
}
