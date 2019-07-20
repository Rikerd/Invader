using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int hp;
    public float movementSpeed;

    public float spawnDistance;
    public float setFireTimer;
    public float setInvincibilityTimer;

    public GameObject bullet;
    public float spriteOffset;
    public float bossBoundary;

    public ScreenShake shake;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;

    private float fireTimer;
    private float invincibilityTimer;
    private bool invincible;
    private Vector3 bulletSpawnPos;

    private Vector2 max;
    private Vector2 min;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        fireTimer = 0f;
        invincible = false;
        invincibilityTimer = 0f;
        sprite = GetComponent<SpriteRenderer>();
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }

    private void Update()
    {
        if (!isPlayerDead())
        {
            if (fireTimer > 0f)
            {
                fireTimer -= Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Space) && fireTimer <= 0f && hp > 0f)
            {
                bulletSpawnPos = transform.position + (transform.right * spawnDistance);

                Instantiate(bullet, bulletSpawnPos, Quaternion.Euler(0f, 0f, -90f));
                fireTimer = setFireTimer;
            }

            if (invincibilityTimer > 0f)
            {
                sprite.enabled = !sprite.enabled;
                invincibilityTimer -= Time.deltaTime;
            }
            else
            {
                sprite.enabled = true;
                invincible = false;
            }
        }
    }
    
    private void FixedUpdate () {
        if (!isPlayerDead())
        {
            Move();
        }
	}

    private void Move()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            input += Vector3.up;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            input += Vector3.down;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            input += Vector3.right;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input += Vector3.left;
        }

        input.Normalize();

        Vector2 movement = input * movementSpeed * Time.deltaTime;
        Vector2 possiblePosition = rb2d.position + movement;

        if (possiblePosition.x <= bossBoundary && possiblePosition.y <= max.y - spriteOffset &&
            possiblePosition.x >= min.x + spriteOffset && possiblePosition.y >= min.y + spriteOffset)
        {
            rb2d.MovePosition(rb2d.position + movement);
        }
    }

    public void takeDamage(int dmg)
    {
        if (!invincible && !isPlayerDead())
        {
            shake.StartShake();
            hp -= dmg;
            invincible = true;
            invincibilityTimer = setInvincibilityTimer;
        }

        if (isPlayerDead())
        {
            hp = 0;
            GetComponent<PolygonCollider2D>().enabled = false;
            sprite.enabled = false;
            GetComponent<ParticleSystem>().Play();
        }
    }

    public bool isPlayerDead()
    {
        return hp <= 0;
    }
}
