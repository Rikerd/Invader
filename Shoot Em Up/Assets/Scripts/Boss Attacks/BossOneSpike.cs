using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneSpike : BossAttack {
    public float spawnSpeed;
    public float spawnTime;

    private bool moving;
    private bool spawning;
    private SpriteRenderer sprite;
    private float alpha;
    private ParticleSystem particle;
    private bool dead;
    private IEnumerator spawn;

    private void Awake()
    {
        baseAwake();

        moving = false;
        spawning = true;
        sprite = GetComponent<SpriteRenderer>();
        alpha = 0f;
        particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        baseStart();

        spawn = SpikeSpawn();
        StartCoroutine(spawn);
    }

    private void FixedUpdate()
    {
        if (spawning)
        {
            alpha += spawnSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        }

        if (moving)
        {
            Vector2 move = transform.right * movementSpeed * Time.deltaTime;
            rb2d.MovePosition(rb2d.position + move);
        }
    }

    IEnumerator SpikeSpawn()
    {
        moving = false;
        spawning = true;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        yield return new WaitForSeconds(spawnTime);
        
        moving = true;
        spawning = false;

        StopCoroutine(spawn);
    }

    public void StopSpike(float dur)
    {
        StopCoroutine(spawn);
        moving = false;
        spawning = false;
        dead = true;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        particle.Play();

        Destroy(gameObject, dur);
    }

    public override void OnBecameInvisible()
    {
        if (!dead)
        {
            Destroy(gameObject);
        }
    }
}
