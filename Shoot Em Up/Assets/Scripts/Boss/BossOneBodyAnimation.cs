using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneBodyAnimation : MonoBehaviour {
    public List<ParticleSystem> particles;

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
    	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            timePassed += Time.deltaTime;
            sprite.color = Color.Lerp(startColor, endColor, timePassed / fadeDuration);
            
            if (timePassed > fadeDuration)
            {
                dead = false;
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
