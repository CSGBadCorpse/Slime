using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SlimeMove : MonoBehaviour
{
    const string moveTrigger = "move";

    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private SlimeAction slimeAction;

    [SerializeField] private Vector3 destinationPos;
    public Sprite newSprite;
    public SpriteRenderer spriteRenderer;


    [SerializeField] private SlimeSpriteLibraryAssetSO slimeSpriteLibraryAssetSO;

    public bool findPlayer = false;
    public bool countDown = false;

    int spriteLibraryAssetIndex = 0;


    private void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        slimeAction = this.transform.GetChild(0).GetComponent<SlimeAction>();
        slimeAction.OnHitted += SlimeAction_OnHitted;
        findPlayer = true;
        countDown = false;

    }

    private void SlimeAction_OnHitted(object sender, System.EventArgs e)
    {
        transform.position = new Vector2(transform.position.x - ((destinationPos.x - transform.position.x)/ Mathf.Abs(destinationPos.x - transform.position.x))*0.5f, 
                                         transform.position.y - ((destinationPos.y - transform.position.y)/Mathf.Abs(destinationPos.y - transform.position.y))*0.5f);
    }

    // Update is called once per frame
    private void Update()
    {//animator.SetFloat(moveTrigger, 0);
        if (countDown)
        {
            StartCoroutine("HoldToFindPlayer");
        }

        if (findPlayer) {
            destinationPos = PlayerController.Instance.transform.position;
            destinationPos.z = 0;
        }


        float h = destinationPos.x - this.transform.position.x;
        float v = destinationPos.y - this.transform.position.y;
        if (h != 0 || v != 0)
        {
            animator.SetFloat(moveTrigger, 0.5f);
        }
        //Debug.Log("h: " + h + " v: " + v);

        if (h > 0)
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (h < 0)
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if ((h > .1f || h < .1f || v > .1f || v < .1f)&&slimeAction.startJump)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, destinationPos, moveSpeed * Time.deltaTime);
            animator.SetFloat(moveTrigger, 0.5f);
        }
        //Debug.Log(slimeAction.animationFinish);
        if (h == 0f && v == 0f && slimeAction.animationFinish)
        {
            animator.SetFloat(moveTrigger, 0f);
            slimeAction.startJump = false;
            countDown = true;
            findPlayer = false;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (spriteLibraryAssetIndex==slimeSpriteLibraryAssetSO.spriteLibraryAssets.Count){
                spriteLibraryAssetIndex = 0;
            }
            
            transform.GetChild(0).GetComponent<SpriteLibrary>().m_SpriteLibraryAsset=slimeSpriteLibraryAssetSO.spriteLibraryAssets[spriteLibraryAssetIndex];
            spriteLibraryAssetIndex++;
            // Debug.Log(sl.m_SpriteLibraryAsset);
            
        }


    }

    IEnumerator HoldToFindPlayer(){
        yield return new WaitForSeconds(1f);
        findPlayer = true;
        countDown = false;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        if(destinationPos.x!=0f&&destinationPos.y!=0f){
            Gizmos.DrawSphere(new Vector2(destinationPos.x,destinationPos.y), 0.1f);
            Gizmos.DrawLine(transform.position,destinationPos);

        }
        
    }
}
