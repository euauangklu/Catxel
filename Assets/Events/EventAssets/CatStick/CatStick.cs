using UnityEngine;

public class CatStick : MonoBehaviour
{
    [SerializeField] private GameObject StickIcon;
    [SerializeField] private GameObject Stick;
    public CatStickEvent eventSource;
    private Camera MainCamera;
    private RandomEventManager RandomEventManager;
    private bool Playing;
    private MiniGame minigame;
    private Animator Animator;
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        RandomEventManager = GameObject.FindGameObjectWithTag("RandomEventManager").GetComponent<RandomEventManager>();
        Animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (!Playing)
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
                        RandomEventManager.isInCatStickEvent = true;
                        Stick.SetActive(true);
                        StickIcon.SetActive(false);
                        float clampedY = Mathf.Clamp(transform.position.y, -1f, 1f);
                        MainCamera.transform.position = new Vector3(transform.position.x,clampedY,MainCamera.transform.position.z);
                        MainCamera.orthographicSize = 0.75f;
                        MiniGame.MiniGameButton.SetActive(true);
                        minigame = GameObject.FindGameObjectWithTag("MiniGameButton").GetComponent<MiniGame>();
                        Playing = true;
                        Animator.SetBool("Playing", true);
                    }
                }
            }
        }

        if (Playing)
        {
            if (eventSource != null && minigame.Done)
            {
                MainCamera.orthographicSize = 1.8f;
                MainCamera.transform.position = new Vector3(0,0,-10);
                eventSource.OnEventDone();
                Playing = false;
                minigame.Done = false;
                Animator.SetBool("Playing", false);
                Destroy(this.gameObject);
            } 
        }
    }
}
