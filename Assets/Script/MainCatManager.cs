using UnityEngine;
using UnityEngine.InputSystem;

public class MainCatManager : MonoBehaviour
{
    public static GameObject MainCat;
    [SerializeField] private DragNDrop _dragNDrop;
    [SerializeField] private Vector3 FloorPos;
    [SerializeField] private float fallSpeed;
    private Animator Animator;
    private bool AlreadyDrag;
    public int CatEXP;
    public int EXPPerEvent = 10;
    private SpriteRenderer cat;
    private float Timer;

    void Awake()
    {
        MainCat = gameObject;
        cat = gameObject.GetComponent<SpriteRenderer>();
        Animator = gameObject.GetComponent<Animator>();
        if (PlayerPrefs.HasKey("CatEXP"))
        {
            CatEXP = PlayerPrefs.GetInt("CatEXP");
        }
        _dragNDrop.EnableDrag = true;
        
    }

    void Update()
    {
        if (CatEXP >= 100)
        {
            cat.color = Color.green;
        }
        
        if (_dragNDrop.isDragging && !AlreadyDrag)
        {
            Animator.SetBool("Hold", true);
            Animator.SetBool("AlreadyHold", true);
            AlreadyDrag = true;
        }
        if (AlreadyDrag)
        {
            Timer += Time.deltaTime;
        }
        if (AlreadyDrag && Timer > 0.1f)
        {
            Animator.SetBool("AlreadyHold", false);
        }
        
        if (!_dragNDrop.isDragging && AlreadyDrag)
        {
            Animator.SetBool("Hold", false);
            if (gameObject.transform.position.y >= FloorPos.y)
            {
                gameObject.transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
                Animator.SetBool("Drop", true);
            }
            else if (gameObject.transform.position.y < FloorPos.y)
            {
                Animator.SetBool("Drop", false);
                AlreadyDrag = false;
                Timer = 0;
            }
        }
        
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CatEXP", CatEXP);
        PlayerPrefs.Save();
    }

    public void ConsoleReset()
    {
        CatEXP = 0;
        PlayerPrefs.SetInt("CatEXP", CatEXP);
        PlayerPrefs.Save();
    }
}
