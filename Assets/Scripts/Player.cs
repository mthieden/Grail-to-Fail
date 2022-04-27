using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject {


    public int wallDamage = 1;

    public int health = 100;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Animator animator;
    private int food;

    private Vector2 moveDirection;

    public Camera maincamera;

    private Vector2 mousePos;

    private Rigidbody2D playerbody;

    public Vector3 offset;


    protected override void Start()
    {
        animator = GetComponent<Animator>();

        playerbody = GetComponent<Rigidbody2D>();

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;


        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        if(moveDirection.x == 0 && moveDirection.y == 0)
            animator.SetBool("playerRunning", false);
        else
            animator.SetBool("playerRunning", true);
        AttemptMove<Wall> (moveDirection.x, moveDirection.y);

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
        foodText.text = "HP:  " + food;

        base.AttemptMove<T>(xDir, yDir);

        LookDirection();
        CheckIfGameOver();

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
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
