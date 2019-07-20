using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float movementSpeed;
    public int damage;

    private Rigidbody2D rb2d;
    private bool hit;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        hit = false;
    }

    private void FixedUpdate()
    {
        if (!hit)
        {
            Vector2 move = transform.up * movementSpeed * Time.deltaTime;
            rb2d.MovePosition(rb2d.position + move);
        }
    }

    public void OnBecameInvisible()
    {
        if (!hit)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            hit = true;
            collision.GetComponentInParent<BossController>().takeDamage(damage);
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            GetComponent<ParticleSystem>().Play();

            Destroy(gameObject, 0.5f);
        }
    }
}
