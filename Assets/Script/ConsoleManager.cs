using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] private int num;
    [SerializeField] private GameObject Console;
    private bool OpenConsole;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && num < 3)
        {
            num += 1;
        }
        if (Input.GetKeyDown(KeyCode.Return) && num >= 3)
        {
            num = 0;
            OpenConsole = false;
        }
        if (num >= 3)
        {
            OpenConsole = true;
        }
        if (OpenConsole)
        {
            Console.SetActive(true);
        }
        else if (!OpenConsole)
        {
            Console.SetActive(false);
        }
    }
}
