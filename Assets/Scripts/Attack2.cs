using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : Attack
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
            enemy.takeDamage(damage * modifier);
            Destroy(gameObject);
        }
        if (collision.collider.name.Contains("boss"))
        {
            //do some boss related damage
            Destroy(gameObject);
        }
    }
}
