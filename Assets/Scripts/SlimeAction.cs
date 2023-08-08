using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class SlimeAction : MonoBehaviour
{
    public event EventHandler<HitInfoEventArgs> OnHitted;
    public event EventHandler OnDie;
    public event EventHandler OnLanded;

    public class HitInfoEventArgs : EventArgs
    {
        public float angle;
    }


    [SerializeField] private int healthMax = 2;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;


    //动画帧参数
    public bool startJump;
    public bool animationFinish;
    private Collider2D[] pos = new Collider2D[1];



    private void Start()
    {
        startJump = false;
        animationFinish = false;
        currentHealth = healthMax;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //实际的碰撞体积在子物体，所以受伤逻辑在底层
    private void GetHit(float a)
    {
        currentHealth--;
        OnHitted?.Invoke(this, new HitInfoEventArgs
        {
            angle = a
        }) ;
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
        // ContactPoint2D[] pos = new ContactPoint2D[10];
        
        if (collision.transform.CompareTag("PlayerHit"))
        {
            collision.GetContacts(pos);
            Vector2 hitPos = pos[0].transform.position;

            Vector2 playerPos = PlayerController.Instance.transform.position;
            float angle = -Mathf.Atan2(hitPos.y - playerPos.y, hitPos.x - playerPos.x)*Mathf.Rad2Deg;


            //float angle = Mathf.Rad2Deg * (hitPos.y - playerPos.y) / (hitPos.x - playerPos.x);

            //float angle = Mathf.Rad2Deg * (hitPos.y - playerPos.y) / (hitPos.x - playerPos.x);
            //Debug.Log(pos[0].transform.position);


            GetHit(angle);
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

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
       // Draw a yellow sphere at the transform's position
    //   Gizmos.color = Color.red;
    ////    if(hitPos!=Vector2.zero&&hitPos!=null){
    //       Gizmos.DrawSphere(hitPos, 0.1f);
    ////    }

    }
#endif



}
