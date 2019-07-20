using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneLance : BossAttack {
    public float initialMoveTime;
    public float warningTime;

    private bool moving;
    private Vector3 lockedOnPlayerPos;
    private Vector3 direction;
    private float angle;
    private IEnumerator warning;
    private ParticleSystem particle;
    private bool dead = false;

    private void Awake()
    {
        baseAwake();

        particle = GetComponent<ParticleSystem>();
    }
    
    private void Start() {
        baseStart();

        lockedOnPlayerPos = playerTrans.position;

        direction = lockedOnPlayerPos - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        warning = LanceWarning();
        StartCoroutine(warning);
	}
	
	private void FixedUpdate() {
	    if (moving)
        {
            Vector2 move = transform.right * movementSpeed * Time.deltaTime;
            rb2d.MovePosition(rb2d.position + move);
        }
	}

    IEnumerator LanceWarning()
    {
        moving = true;
        yield return new WaitForSeconds(initialMoveTime);
        moving = false;
        yield return new WaitForSeconds(warningTime);
        moving = true;

        StopCoroutine(warning);
    }

    public void StopLance(float dur)
    {
        StopCoroutine(warning);
        moving = false;
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
