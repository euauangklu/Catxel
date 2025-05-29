using System;
using UnityEngine;

public class CatBall : MonoBehaviour
{
    public CatPlayAction CatPlayAction;
    private SpriteRenderer SpriteRenderer;
    private void Awake()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        SpriteRenderer.enabled = false;
    }
    
    public void OnCollisionExit2D(Collision2D other)
    {
        SpriteRenderer.enabled = true;
    }
}
