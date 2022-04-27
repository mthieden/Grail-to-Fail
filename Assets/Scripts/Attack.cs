using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{   
    public Transform Weapon;
    public GameObject Attack1;
    public GameObject Attack2;

    public GameObject Attack5;

    private GameObject currentweapon; 

    private Camera maincamera;

    private List<GameObject> weaponlist;
 
    private int x = 1;

    public int currentdamagemodifier = 1;
    public float Attackforce = 20f;

    void Start()
    {
        maincamera = FindObjectOfType<Camera>();
        currentweapon = Attack1;
        weaponlist = new List<GameObject>();
        weaponlist.Add(Attack1);
        weaponlist.Add(Attack2);
        weaponlist.Add(Attack5);
    }
    void FixedUpdate()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (weaponlist != null)
            {
                if (x < weaponlist.Count)
                {
                    currentweapon = weaponlist[x];
                    x++;
                }
                else 
                {
                    x = 0;
                    currentweapon = weaponlist[x];
                    x++;
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentweapon != null)
            {
                CurrentAttack();
            }
        }

    }
    private void CurrentAttack()
    {
        Vector3 lookDir = maincamera.ScreenToWorldPoint(Input.mousePosition);

        lookDir.z = 0;

        lookDir = (lookDir - Weapon.transform.position).normalized;
        float mouseangle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        if (currentweapon.name == "Attack1")
        {
           GameObject attack = Instantiate(currentweapon, Weapon.position, Weapon.rotation);
           Rigidbody2D rb2d = attack.GetComponent<Rigidbody2D>();
           rb2d.AddForce(lookDir * Attackforce, ForceMode2D.Impulse);
        }
        if (currentweapon.name == "Attack2")
        {
           for (int i = 1; i < 4; i++){ 
           GameObject attack = Instantiate(currentweapon, Weapon.position,  Weapon.rotation);
           Rigidbody2D rb2d = attack.GetComponent<Rigidbody2D>();
           attack.transform.rotation = Quaternion.Euler(0,0,mouseangle-(i*45));
           rb2d.AddForce(attack.transform.up * Attackforce, ForceMode2D.Impulse);
           }
        }
        if (currentweapon.name == "Attack5")
        {
            GameObject attack = Instantiate(currentweapon, Weapon.position+ new Vector3 (0.5F,0.5F,0), Weapon.rotation);
        }
    }

}
