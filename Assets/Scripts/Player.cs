using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public static Player instance { get; private set; }

    public int wallDamage = 1;

    
    
    public float restartLevelDelay = 1f;
    private bool invincible = false;
    public Text healthText;
    /*
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
    */


    private Animator animator;

    public int maxHealth;
    private int health;
    public int Health 
    {
        get { return health; } 
        set { health = value; }
    }

    private Vector2 moveDirection;

    public Camera maincamera;

    private Vector2 mousePos;

    private Rigidbody2D playerbody;

    public Vector3 offset;

    private int timeSinceLastDamage = 30;

    void Awake()
    {

        Debug.Log("PLAYER WAKING UP");
        health = maxHealth;
    }

    private void OnEnable()
    {
        Debug.Log("ENABLING, INSTANCE IS: " + this.name.ToString());
        instance = this;
    }

    protected override void Start()
    {
        
        animator = GetComponent<Animator>();
        playerbody = GetComponent<Rigidbody2D>();
        //health = GameManager.instance.playerHealth;
        healthText.text = "HP: " + health;

        Debug.Log("PALYER START, CURRENT HEALTH: " + health);

        base.Start();

    }

    private void FixedUpdate()
    {
        if (moveDirection.x == 0 && moveDirection.y == 0)
            animator.SetBool("playerRunning", false);
        else
            animator.SetBool("playerRunning", true);
        AttemptMove<Wall>(moveDirection.x, moveDirection.y);

        if (timeSinceLastDamage < 30) timeSinceLastDamage++;
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInputs();
    }

    private void OnDisable()
    {
        Debug.Log("DISABLING, INSTANCE WAS: " + this.ToString());
        instance = null;
    }

    private void ProcessInputs()
    {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        maincamera.transform.position = playerbody.transform.position + offset;

        mousePos = maincamera.ScreenToWorldPoint(Input.mousePosition);

        moveDirection = new Vector2(moveX, moveY).normalized;

    }
    protected override void AttemptMove <T> (float xDir, float yDir)
    {
        //food--;
        healthText.text = "HP:  " + health;

        base.AttemptMove<T>(xDir, yDir);

        LookDirection();

    }

    private void LookDirection()
    {
        Vector2 lookDir = mousePos - playerbody.position;
        float mouseangle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if ((mouseangle >= 0 && mouseangle <= 90) || (mouseangle <= 0 && mouseangle >= -90 ))
        {
            playerbody.transform.localScale = new Vector3 (1,1,1);
        }
        else
        {
            playerbody.transform.localScale = new Vector3 (-1,1,1);
        }
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
        }
    }

    private void Restart()
    {
        GameManager.instance.NextLevel();
    }

    public void TakeDamage(int damage)
    {
        if (timeSinceLastDamage >= 30)
        {
            timeSinceLastDamage = 0;
            //animator.SetTrigger("playerHit");
            health -= damage;
            healthText.text = "HP: " + health;
            CheckIfGameOver();
        }
    }

    public void setInvincible ()
    {
        Debug.Log("BECOMING INVINCIBLE!");
        invincible = true;
    }

    public void removeInvincible ()
    {
        invincible = false;
    }

    private void CheckIfGameOver()
    {
        Debug.Log("CHECKING IF GAME IS OVER??");
        if (health <= 0)
        {
            //SoundManager.instance.PlaySingle(gameOverSound);
            //SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
