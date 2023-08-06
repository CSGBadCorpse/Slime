using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Lean.Pool;

public class SlimeMove : MonoBehaviour,IPoolable
{
    const string moveTrigger = "move";

    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private SlimeAction slimeAction;

    [SerializeField] private Vector3 destinationPos;
    [SerializeField] private SpriteRenderer spriteRenderer;


    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private SlimeSpriteLibraryAssetSO slimeSpriteLibraryAssetSO;
    [SerializeField] private GameObject deathPartical;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    [SerializeField] private int scoreValue;
    //0 hit 1 death 2 step

    private bool findPlayer = false;
    private bool countDown = false;


    public void OnSpawn()//用对象池生成时，初始化操作
    {
        //audioClips.Clear();
        audioSource.clip = null;
        slimeAction.gameObject.SetActive(true);
        deathPartical.SetActive(false);
        findPlayer = true;
        countDown = false;
        animator.SetFloat(moveTrigger, 0f);
        slimeAction.startJump = false;
        RandomSkin();
    }

    public void OnDespawn()//对象池消除对象时，重置一些参数
    {
        slimeAction.OnSlimeDespawn();
    }

    private void Start()
    {
        slimeAction.OnHitted += SlimeAction_OnHitted;
        slimeAction.OnLanded += SlimeAction_OnLanded;
        slimeAction.OnDie += SlimeAction_OnDie;
        findPlayer = true;
        countDown = false;
        
    }

    private void SlimeAction_OnLanded(object sender, System.EventArgs e)
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    private void SlimeAction_OnDie(object sender, System.EventArgs e)
    {
        Die();//史莱姆的生命为0时，死亡
    }

    private void SlimeAction_OnHitted(object sender, System.EventArgs e)
    {
        //史莱姆受到攻击，向来的方向退0.5f 这里可以统一修改攻击力
        //因为实际受击范围在子物体，但是实际的整体移动在父物体，所以用监听事件来做处理
        transform.position = new Vector2(transform.position.x - ((destinationPos.x - transform.position.x)/ Mathf.Abs(destinationPos.x - transform.position.x))*0.5f, 
                                         transform.position.y - ((destinationPos.y - transform.position.y)/Mathf.Abs(destinationPos.y - transform.position.y))*0.5f);
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }


    private void Update()
    {
        SlimeMovement();
    }

    private void SlimeMovement()
    {
        if (countDown)//起一个协程，计算等待的时间找主角
        {
            StartCoroutine("HoldToFindPlayer");
        }

        if (findPlayer)//如果史莱姆没到达目的地，就要时刻寻找主角的位置
        {
            destinationPos = PlayerController.Instance.transform.position;
            destinationPos.z = 0;
        }


        float h = destinationPos.x - this.transform.position.x;
        float v = destinationPos.y - this.transform.position.y;
        if (h != 0 || v != 0)
        {
            animator.SetFloat(moveTrigger, 0.5f);
        }

        //反转
        if (h > 0)
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (h < 0)
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if ((h > .1f || h < .1f || v > .1f || v < .1f) && slimeAction.startJump)//只有动画帧在起跳的时候才能移动
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, destinationPos, moveSpeed * Time.deltaTime);
            animator.SetFloat(moveTrigger, 0.5f);
        }

        if (h == 0f && v == 0f && slimeAction.animationFinish)//如果到达目的地
        {
            animator.SetFloat(moveTrigger, 0f);
            slimeAction.startJump = false;
            countDown = true;//史莱姆停住，等一段时间再开始找主角
            findPlayer = false;//停止寻找主角
        }
    }

    IEnumerator HoldToFindPlayer(){
        yield return new WaitForSeconds(1f);//史莱姆如果到达目的地，等待1s开始寻找主角，看情况可以删掉，因为主角会受击后退
        findPlayer = true;
        countDown = false;
    }
    private void RandomSkin()//生成的时候按照SO随机生成皮肤
    {
        spriteLibrary.m_SpriteLibraryAsset = 
            slimeSpriteLibraryAssetSO.spriteLibraryAssets[
                Random.Range(0, slimeSpriteLibraryAssetSO.spriteLibraryAssets.Count)
            ];
    }

    private void Die()
    {
        Score.Instance.AddScore(scoreValue);

        animator.SetFloat("move", 0f);
        slimeAction.startJump = false;
        audioSource.clip = audioClips[1];
        audioSource.Play();
        LeanGameObjectPool pool = GameManager.Instance.GetPool();
        deathPartical.SetActive(true);
        
        slimeAction.gameObject.SetActive(false);
        pool.Despawn(this.gameObject,1f);
    }
#if UNITY_EDITOR

    //void OnDrawGizmos()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.red;
    //    if(destinationPos.x!=0f&&destinationPos.y!=0f){
    //        Gizmos.DrawSphere(new Vector2(destinationPos.x,destinationPos.y), 0.1f);
    //        Gizmos.DrawLine(transform.position,destinationPos);

    //    }

    //}
#endif
}
