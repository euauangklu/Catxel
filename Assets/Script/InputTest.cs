using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch");
        }
    }
}
