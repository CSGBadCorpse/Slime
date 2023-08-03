using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    const string moveTrigger = "move";


    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private SlimeAction slimeAction;

    [SerializeField] private Vector3 destinationPos;
    public Sprite newSprite;
    public SpriteRenderer spriteRenderer;


    [SerializeField] private SlimePrefabs prefabsList;

    float h = 0;
    float v = 0;
    bool clicked = false;
    // Start is called before the first frame update
    private void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        slimeAction = this.transform.GetChild(0).GetComponent<SlimeAction>();
    }

    // Update is called once per frame
    private void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        if (Input.GetMouseButtonDown(0))
        {
            destinationPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            destinationPos.z = 0;

            h = (destinationPos.x - transform.position.x); // Mathf.Abs(destinationPos.x - transform.position.x);
            v = (destinationPos.y - transform.position.y); // Mathf.Abs(destinationPos.y - transform.position.y);
            clicked = true;
        }
        if (h < 0f)
        {
            transform.localScale = new Vector3(-1.0f, 1, 1);
        }
        else if (h > 0f)
        {
            transform.localScale = new Vector3(1.0f, 1, 1);
        }
        if (h != 0f || v != 0f)
        {
            animator.SetFloat(moveTrigger, Mathf.Abs(h) + Mathf.Abs(v));
        }
        if (Mathf.Abs(transform.position.x - destinationPos.x) <= 0.1f && Mathf.Abs(transform.position.y - destinationPos.y) <= 0.1f)
        {
            animator.SetFloat(moveTrigger, 0);
        }

        if (slimeAction.startJump)
        {
            if (clicked && (Mathf.Abs(transform.position.x - destinationPos.x) > 0.1f && Mathf.Abs(transform.position.y - destinationPos.y) > 0.1f))
            {
                this.transform.position = new Vector2(transform.position.x + h / Mathf.Abs(h) * moveSpeed * Time.deltaTime,
                                                  transform.position.y + v / Mathf.Abs(v) * moveSpeed * Time.deltaTime);
            }
            else if (clicked && (Mathf.Abs(transform.position.x - destinationPos.x) > 0.1f))
            {
                this.transform.position = new Vector2(transform.position.x + h / Mathf.Abs(h) * moveSpeed * Time.deltaTime,
                                                  transform.position.y);
            }
            else if (clicked && (Mathf.Abs(transform.position.y - destinationPos.y) > 0.1f))
            {
                this.transform.position = new Vector2(transform.position.x,
                                                 transform.position.y + v / Mathf.Abs(v) * moveSpeed * Time.deltaTime);
            }
        }

        

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    transform.GetChild(0).gameObject.SetActive(false);
        //    Transform slime = Instantiate(prefabsList.slimes[1],this.transform);
        //    //Debug.Log(prefabsList.slimes[0]);

        //    //slime.SetParent(this.transform);
        //    animator = transform.GetChild(1).GetComponent<Animator>();
        //}


    }
}
