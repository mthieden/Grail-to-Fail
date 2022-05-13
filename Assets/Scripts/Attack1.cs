using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : Attack
{
    Enemy enemy;
    private int damage = 10;
    private int modifier;
    List<Enemy> hitEnemies = new List<Enemy>();

    void Start()
    {
        modifier = base.currentdamagemodifier;
        //Debug.Log("MODIFIER IS: " + modifier);
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
            if (!hitEnemies.Contains(enemy))
            {
                enemy.takeDamage(damage * modifier);
                hitEnemies.Add(enemy);
            }
        }
        if (collision.collider.name.Contains("boss"))
        {
            //do some boss related damage
            Destroy(gameObject);
        }
    }
}
