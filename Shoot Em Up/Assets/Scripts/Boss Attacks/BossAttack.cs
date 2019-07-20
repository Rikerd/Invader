using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {
    public int damage;
    public float movementSpeed;

    protected Transform playerTrans;
    protected Rigidbody2D rb2d;
    
    public void baseAwake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
	public void baseStart()
    {
        playerTrans = GameObject.Find("Player").transform;
	}

    public virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().takeDamage(damage);
        }
    }
}
