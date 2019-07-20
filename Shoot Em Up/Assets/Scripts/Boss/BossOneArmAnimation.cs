using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneArmAnimation : MonoBehaviour {
    public float rotationSpeed;
    public float radius;
    public Transform center;
    
    public List<ParticleSystem> particles;

    private float angle;
    private bool dead = false;
    private SpriteRenderer sprite;
    private Color startColor;
    private Color endColor;
    private float timePassed;
    private float fadeDuration;

    private void Awake()
    {
        dead = false;

        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (!dead)
        {
            angle += rotationSpeed * Time.deltaTime;

            Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
            transform.position = center.position + offset;
        } else
        {
            timePassed += Time.deltaTime;
            sprite.color = Color.Lerp(startColor, endColor, timePassed / fadeDuration);
            
            if (timePassed > fadeDuration)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void StartDeath(float dur)
    {
        dead = true;
        fadeDuration = dur;

        startColor = sprite.color;
        endColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
}
