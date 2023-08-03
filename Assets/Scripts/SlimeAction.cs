using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAction : MonoBehaviour
{
    public int healthMax = 10;

    public int currentHealth ;

    public bool startJump;
    // public bool aniFinish;

    private void Start()
    {
        startJump = false;
        currentHealth = healthMax;
        // aniFinish = false;
    }

    public void Jump()
    {
        startJump = true;
    }

    public void Land()
    {
        startJump = false;
    }

    // public void AnimationStart(){
    //     aniFinish = false;
    // }
    // public void AnimationFinish(){
    //     aniFinish = true;
    // }

    void OnTriggerEnter2D(Collider2D col){
        if(col.transform.CompareTag("Player")){
            currentHealth--;
        }
    }
}
