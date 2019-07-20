using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BossController : MonoBehaviour {

    public int health;
    public List<GameObject> attacks;

    public List<float> phaseTimes;
    public List<int> phaseHealthLimit;

    public int bossDamage;

    private GameObject player;

    protected Rigidbody2D rb2d;
    protected PlayerController playerControl;
    protected Transform playerTrans;

    public void baseAwake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
	public void baseStart() {
        player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerController>();
        playerTrans = player.transform;
	}

    public virtual void takeDamage(int playerDamage)
    {
        health -= playerDamage;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().takeDamage(bossDamage);
        }
    }
}
