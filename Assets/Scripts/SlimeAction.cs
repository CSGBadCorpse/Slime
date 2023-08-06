using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class SlimeAction : MonoBehaviour
{
    public event EventHandler OnHitted;
    public event EventHandler OnDie;
    public event EventHandler OnLanded;


    [SerializeField] private int healthMax = 2;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;


    //动画帧参数
    public bool startJump;
    public bool animationFinish;

    

    private void Start()
    {
        startJump = false;
        animationFinish = false;
        currentHealth = healthMax;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //实际的碰撞体积在子物体，所以受伤逻辑在底层
    private void GetHit()
    {
        currentHealth--;
        OnHitted?.Invoke(this, EventArgs.Empty);
        spriteRenderer.color = Color.red;
        StartCoroutine("HitChangeColor");
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnSlimeDespawn()
    {
        spriteRenderer.color = Color.white;
        transform.localPosition = Vector2.zero;
        currentHealth = healthMax;
    }
    public void OnSlimeSpawn()
    {
        startJump = false;
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

    #region 动画帧事件
    public void Jump()
    {
        startJump = true;
    }

    public void Land()
    {
        OnLanded?.Invoke(this, EventArgs.Empty);
        startJump = false;
    }

    public void StartAnim()
    {
        animationFinish = false;
    }
    public void FinishAnim()
    {
        animationFinish = true;
    }
    #endregion

}
