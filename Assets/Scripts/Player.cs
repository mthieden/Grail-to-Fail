using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public static Player instance { get; private set; }

    public int maxHealth;
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public Text healthText;
    public Camera maincamera;
    public Vector3 offset;
    public LayerMask blockingLayer;
    public float completeLevelDelay = 1f;
    public float moveSpeed;

    private Vector2 mousePos;
    private Vector2 moveDirection;
    private Rigidbody2D playerbody;
    private Animator animator;    

    private bool invincible = false;
    private int timeSinceLastDamage = 30;

    void Awake()
    {
        health = maxHealth;
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerbody = GetComponent<Rigidbody2D>();
        healthText.text = "HP: " + health;
    }

    private void FixedUpdate()
    {
        if (moveDirection.x != 0 || moveDirection.y != 0)
            animator.SetBool("playerRunning", true);
        else
            animator.SetBool("playerRunning", false);
        Move(moveDirection.x, moveDirection.y);
        LookDirection();

        if (timeSinceLastDamage < 30) timeSinceLastDamage++;
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInputs();
    }

    private void OnDisable()
    {
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

    protected void Move(float xDir, float yDir)
    {
        playerbody.velocity = new Vector2(xDir * moveSpeed, yDir * moveSpeed);
    }

    private void LookDirection()
    {
        Vector2 lookDir = mousePos - playerbody.position;
        float mouseangle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if ((mouseangle >= 0 && mouseangle <= 90) || (mouseangle <= 0 && mouseangle >= -90 )) //(lookDir.x > 0)
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
            Invoke("CompleteLevel", completeLevelDelay);
        }
    }

    private void CompleteLevel()
    {
        GameManager.instance.NextLevel();
    }

    public void TakeDamage(int damage)
    {
        if(!invincible)
        {
            if (timeSinceLastDamage >= 30)
            {
                timeSinceLastDamage = 0;
                animator.SetTrigger("playerHit");
                health -= damage;
                healthText.text = "HP: " + health;
                CheckIfGameOver();
            }
        }
    }

    public void setInvincible ()
    {
        invincible = true;
    }

    public void removeInvincible ()
    {
        invincible = false;
    }

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            //SoundManager.instance.PlaySingle(gameOverSound);
            //SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
