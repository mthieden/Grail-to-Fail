using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : Attack
{
    Enemy enemy;
    private int damage = 10;
    private int modifier;
    
    void Start()
    {
        modifier = base.currentdamagemodifier;
    }
    void FixedUpdate()
    {
        transform.Rotate(0f,0f,20f, Space.Self);
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
