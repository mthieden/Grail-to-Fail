using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    public float moveSpeed;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        
    }

    protected bool Move(float xDir, float yDir)
    {
            rb2D.velocity = new Vector2(xDir * moveSpeed, yDir * moveSpeed);
            return true;
    }

    protected virtual void AttemptMove<T>(float xDir, float yDir)
        where T : Component

    {
    Move(xDir, yDir);

    }
}
