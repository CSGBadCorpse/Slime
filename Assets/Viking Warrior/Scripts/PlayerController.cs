using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event EventHandler<PlayerHealthEventArgs> OnPlayerHealthChanged;
    public event EventHandler OnPlayerDead;


    [SerializeField] private List<AudioClip> audioClips;
    //0 hitted 1 death
    [SerializeField] private AudioSource audioSource;





    public class PlayerHealthEventArgs : EventArgs
    {
        public float healthProgress;
    }

    public class EnemyPosEventArgs : EventArgs {
        public float x;
        public float y;
    }

    
    [Header("Character Attributes:")]
    public float CHARACTER_MOVE_SPEED = 1.0f;
    public int CHARACTER_MAX_HEALTH = 100;
    [SerializeField]private int currentPlayerHealth;

    [Space]
    [Header("Character Stats:")]
    public Vector2 movementDirection;
    public float movementSpeed;

    [Space]
    [Header("References:")]
    public Rigidbody2D rb;
    public Animator animator;

    [SerializeField] private GetHit getHit;
    private bool playerDead;

    [SerializeField] private Transform playerSprite;

    private void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start(){
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        currentPlayerHealth = CHARACTER_MAX_HEALTH;
        getHit.OnPlayerHit += GetHit_OnPlayerHit;
        playerDead = false;
    }

    private void GameManager_OnGameRestart(object sender, EventArgs e)
    {
        playerDead = false;
        currentPlayerHealth = CHARACTER_MAX_HEALTH;
        animator.SetTrigger("Respawn");
        getHit.gameObject.SetActive(true);
    }

    private void GetHit_OnPlayerHit(object sender, EnemyPosEventArgs e)
    {
        float x = e.x;
        float y = e.y;
        if(!playerDead)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), -0.8f);
        }
        DiffHealth(-2);
    }


    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        Move();
        Animate();


        // Flips the player if headed in other direction
        if (movementDirection.x < 0){
            // Debug.Log("Facing Left");
            playerSprite.transform.localScale = new Vector3(-1,1,1);
        } else if (movementDirection.x>0){
            // Debug.Log("Facing Right");
            playerSprite.transform.localScale = new Vector3(1,1,1);
        }
        
    }

    void ProcessInputs (){
        // Stores the direction (x,y) into a variable from user inputs 
        if (!playerDead)
        {
            movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        // Caps the movement speed to the same in all directions, otherwise travelling diagonally would be faster than non-diagonally
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();

        if (!playerDead)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                //aplsdjfodashjfk
                //if (movementSpeed < 0.5f) {
                //    //Attack("Slash");
                //    Attack("Thrust");

                //    //adsfjdlksaj
                //}
                //else {
                Attack("Slash");

                //}
            }
        }
         

 
    }

    void Move(){
        rb.velocity = movementDirection * movementSpeed * CHARACTER_MOVE_SPEED;
    }   

    void Animate(){
        if (movementDirection != Vector2.zero) {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
        animator.SetFloat("Speed", movementSpeed);
    }
    
    void Attack(string attackType){
        if (attackType == "Slash"){
            animator.SetTrigger("Slash");
        }
        else if (attackType =="Thrust"){
            animator.SetTrigger("Thrust");
        }
    
    
    }

    void DiffHealth(int diffHealth){
        currentPlayerHealth = Mathf.Clamp(currentPlayerHealth + diffHealth, 0, CHARACTER_MAX_HEALTH);

        OnPlayerHealthChanged?.Invoke(this,
            new PlayerHealthEventArgs
            {
                healthProgress = (float)currentPlayerHealth / (float)CHARACTER_MAX_HEALTH
            });

        if (currentPlayerHealth <= 0) {
            rb.velocity = Vector2.zero;
            movementSpeed = 0f;
            movementDirection = Vector2.zero;

            animator.SetTrigger("Death");

            currentPlayerHealth = CHARACTER_MAX_HEALTH;
            PlayerDeath();
            getHit.gameObject.SetActive(false);
            playerDead = true;
            OnPlayerDead?.Invoke(this, EventArgs.Empty);
        }
        else if (diffHealth < 0){
            animator.SetTrigger("Hit");
            PlayerHitted();
        }
    }
    public void PlayerDeath()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void PlayerHitted()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

}
