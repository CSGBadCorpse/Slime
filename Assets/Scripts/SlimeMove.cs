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

    float h = 0;
    float v = 0;
    bool clicked = false;
    bool findPlayer = false;

    int spriteLibraryAssetIndex = 0;
    // Start is called before the first frame update
    private void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        slimeAction = this.transform.GetChild(0).GetComponent<SlimeAction>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Mathf.Abs(PlayerController.Instance.transform.position.x - transform.position.x)>0f||Mathf.Abs(PlayerController.Instance.transform.position.y - transform.position.y)>0f
        if(!findPlayer){
           StartCoroutine("HoldToFindPlayer");
        }

        if(findPlayer){
            destinationPos = PlayerController.Instance.transform.position;
        }
        
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        // if (Input.GetMouseButtonDown(0))
        // {
        //     destinationPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     destinationPos.z = 0;

        //     h = (destinationPos.x - transform.position.x); // Mathf.Abs(destinationPos.x - transform.position.x);
        //     v = (destinationPos.y - transform.position.y); // Mathf.Abs(destinationPos.y - transform.position.y);
        //     clicked = true;
        // }
        // if (h < 0f)
        // {
        //     transform.localScale = new Vector3(-1.0f, 1, 1);
        // }
        // else if (h > 0f)
        // {
        //     transform.localScale = new Vector3(1.0f, 1, 1);
        // }
        if ( destinationPos.x - transform.position.x < 0f)
        {
            transform.localScale = new Vector3(-1.0f, 1, 1);
        }
        else if ( destinationPos.x - transform.position.x> 0f)
        {
            transform.localScale = new Vector3(1.0f, 1, 1);
        }
        // if (Mathf.Abs(transform.position.x - destinationPos.x) != 0f || Mathf.Abs(transform.position.y - destinationPos.y) != 0f)
        // {
        //     animator.SetFloat(moveTrigger, Mathf.Abs(h) + Mathf.Abs(v));
        //     // slimeAction.startJump = true;
        // }
        if (Mathf.Abs(transform.position.x - destinationPos.x) != 0f || Mathf.Abs(transform.position.y - destinationPos.y) != 0f)
        {
            animator.SetFloat(moveTrigger, Mathf.Abs(transform.position.x - destinationPos.x) + Mathf.Abs(transform.position.y - destinationPos.y));
            // slimeAction.startJump = true;
        }
        if (Mathf.Abs(transform.position.x - destinationPos.x) <= 0f && Mathf.Abs(transform.position.y - destinationPos.y) <= 0f)
        {
            if(slimeAction.startJump == false){
                animator.SetFloat(moveTrigger, 0);
                // findPlayer = false;
            }
            
            
        }

        if (slimeAction.startJump)
        {
            //clicked && 
            if ((Mathf.Abs(transform.position.x - destinationPos.x) > 0f || Mathf.Abs(transform.position.y - destinationPos.y) > 0f))
            {
                transform.position = Vector2.MoveTowards(transform.position,destinationPos,moveSpeed*Time.deltaTime);
                // this.transform.position = new Vector2(transform.position.x + h / Mathf.Abs(h) * moveSpeed * Time.deltaTime,
                //                                   transform.position.y + v / Mathf.Abs(v) * moveSpeed * Time.deltaTime);
            }
            else if (clicked && (Mathf.Abs(transform.position.x - destinationPos.x) > 0f))
            {
                transform.position = Vector2.MoveTowards(transform.position,destinationPos,moveSpeed*Time.deltaTime);
                // this.transform.position = new Vector2(transform.position.x + h / Mathf.Abs(h) * moveSpeed * Time.deltaTime,
                //                                   transform.position.y);
            }
            else if (clicked && (Mathf.Abs(transform.position.y - destinationPos.y) > 0f))
            {
                transform.position = Vector2.MoveTowards(transform.position,destinationPos,moveSpeed*Time.deltaTime);
                // this.transform.position = new Vector2(transform.position.x,
                //                                  transform.position.y + v / Mathf.Abs(v) * moveSpeed * Time.deltaTime);
            }
            
            
        }
        if((Mathf.Abs(transform.position.x - destinationPos.x) <= 0f && Mathf.Abs(transform.position.y - destinationPos.y) <= 0f)){
            findPlayer = false;
            slimeAction.startJump = false;
        }

        


        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if(spriteLibraryAssetIndex==slimeSpriteLibraryAssetSO.spriteLibraryAssets.Count){
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
