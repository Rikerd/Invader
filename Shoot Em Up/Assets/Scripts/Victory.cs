using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {
    public GameManager gm;
    public int index;
    public float fadeDuration;

    private float timePassed;
    private SpriteRenderer sprite;
    private Color startColor;
    private Color endColor;
    private PolygonCollider2D collide;
    private bool fadeIn;

    private void Start()
    {
        timePassed = 0f;
        sprite = GetComponent<SpriteRenderer>();
        collide = GetComponent<PolygonCollider2D>();
        collide.enabled = false;

        startColor = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        endColor = sprite.color;

        sprite.color = startColor;

        fadeIn = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            timePassed += Time.deltaTime;
            sprite.color = Color.Lerp(startColor, endColor, timePassed / fadeDuration);

            if (timePassed > fadeDuration)
            {
                collide.enabled = true;
                fadeIn = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.LoadVictoryScene(index);
        }
    }
}
