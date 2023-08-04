using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlimeAction : MonoBehaviour
{
    public event EventHandler OnHitted;


    public int healthMax = 10;

    public int currentHealth ;

    public bool startJump;

    public bool animationFinish;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        startJump = false;
        animationFinish = false;
        currentHealth = healthMax;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Jump()
    {
        startJump = true;
    }

    public void Land()
    {
        startJump = false;
    }
    
    public void StartAnim()
    {
        animationFinish = false;
    }
    public void FinishAnim()
    {
        Debug.Log("Finished");
        animationFinish = true;
    }
    private void GetHit()
    {
        currentHealth--;
        OnHitted?.Invoke(this, EventArgs.Empty);
        spriteRenderer.color = Color.red;
        StartCoroutine("HitChangeColor");
        //spriteRenderer.color = Color.red;

    }

    IEnumerator HitChangeColor()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("PlayerHit"))
        {
            GetHit();
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.transform.CompareTag("PlayerHit"))
    //    {
    //        GetHit();
    //    }
    //}

}
