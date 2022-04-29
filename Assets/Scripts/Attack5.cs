using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack5 : Attack
{
     Enemy enemy;
    private int damage = 10;
    private int modifier;
    public Player player;

    void Start()
    {
        modifier = base.currentdamagemodifier;
    }
    void FixedUpdate()
    {
        player = GameObject.FindObjectOfType<Player>();
        transform.RotateAround(player.transform.position, new Vector3 (0,0,1), 180f*Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("wall"))
        {
            Destroy(gameObject);
        }
        if (collision.collider.name.Contains("Enemy"))
        {
            enemy = collision.collider.GetComponent<Enemy>();
            enemy.hp -= damage*modifier;
            Debug.Log(enemy.hp);
            Destroy(gameObject);
            if (enemy.hp <= 0)
            {
                GameManager.Destroy(enemy.gameObject);
            }
        }
        if (collision.collider.name.Contains("boss"))
        {
            //do some boss related damage
            Destroy(gameObject);
        }
    }
}
