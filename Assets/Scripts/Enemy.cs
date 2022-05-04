using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 200f;
    public float nextWayPointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    public int hp;
    public int damage;
    public float detectionAoe;
    public float attackSpeed;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    private Transform player;

    private Animator animator;

    public float stopdistance;


protected virtual void Start()
{
    animator = GetComponent<Animator>();
    seeker = GetComponent<Seeker>();
    player = GameObject.FindObjectOfType<Player>().transform;
    boxCollider = GetComponent<BoxCollider2D>();
    rb2D = GetComponent<Rigidbody2D>();

    InvokeRepeating("UpdatePath", 0f, .5f);


}
protected virtual void UpdatePath()
{
    if (seeker.IsDone())
        seeker.StartPath(rb2D.position, player.position, OnPathComplete);
}
protected virtual void OnPathComplete(Path p)
{
    if(!p.error)
    {
        path = p;
        currentWaypoint = 0;
    }
}
protected virtual void FixedUpdate()
{
    if (path == null)
    {
        return;
    }
    if (currentWaypoint >= path.vectorPath.Count)
    {
        reachedEndOfPath = true;
        return;
    } else
    {
        reachedEndOfPath = false;
    }

    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2D.position).normalized;
    Vector2 force = direction * moveSpeed * Time.deltaTime;
    Move(force);
    float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

    if (distance < nextWayPointDistance)
    {
        currentWaypoint++;
    }
    LookDirection(force);

}
protected virtual void Move(Vector2 force)
{
    if((Vector2.Distance(transform.position, player.position) < detectionAoe) & (Vector2.Distance(transform.position, player.position) > stopdistance)){
        rb2D.AddForce(force);
        animator.SetBool("enemyRunning", true);
    }
    if (Vector2.Distance(transform.position, player.position) < stopdistance){
        animator.SetBool("enemyRunning", false);
        animator.SetBool("enemyHit", true);
    } 
    if (Vector2.Distance(transform.position, player.position) > stopdistance){
        animator.SetBool("enemyHit", false);
    }
}
protected virtual void Attack(){
    GameObject.FindObjectOfType<Player>().LoseHealth(damage);
}
protected virtual void LookDirection(Vector2 force)
{
    if (force.x >= 0.01f)
    {
        rb2D.transform.localScale = new Vector3 (1f,1f,1f);
    }
    else if (force.x <= -0.01f)
    {
        rb2D.transform.localScale = new Vector3 (-1f,1f,1f);
    }
}
}

