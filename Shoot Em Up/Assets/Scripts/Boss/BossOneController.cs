using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneController : BossController {
    public GameObject victory;

    public float movementSpeed;
    public float boundaryOffset;

    public float initialMovementDuration;

    public float phaseOneLanceXOffset;
    public float phaseOneLanceYOffset;

    public float phaseTwoSpikeXOffset;
    public float phaseTwoSpikeYOffsetRange;

    public List<float> phaseThreeMovementDuration;
    public SpriteRenderer warningSprite;
    public float warningSpeed;

    public List<SpriteRenderer> bodyPartsSprites;

    public float armFadeDuration;
    public GameObject topArm;
    public GameObject botArm;
    public float bodyFadeDuration;
    public GameObject body;
    public GameObject middle;
    public GameObject mark;

    private Vector2 max;
    private Vector2 min;

    private bool initialMovement;

    private int direction;
    private float phaseTwoSpikeYOffset;

    private Color originalColor;

    private bool moving;
    private bool[] phaseThreePartActivated;
    private float timePassed;
    private Vector2 originalPos;
    private float warningAlpha = 0f;
    private bool phaseTransition;

    private int maxHealth;

    private GameObject[] enemyObjects;

    private IEnumerator routine;

    private void Awake()
    {
        baseAwake();

        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        direction = Random.Range(0f, 1f) > 0.5 ? 1 : -1;
        moving = true;

        phaseThreePartActivated = new bool[phaseThreeMovementDuration.Count];

        for (int i = 0; i < phaseThreeMovementDuration.Count; i++)
        {
            phaseTimes[2] += phaseThreeMovementDuration[i];
        }

        phaseTransition = false;

        maxHealth = health;
    }
    
    private void Start () {
        baseStart();

        routine = BossRoutine();
        StartCoroutine(routine);
    }

    private void Update()
    {
        if (health > phaseHealthLimit[0])
        {
            foreach(SpriteRenderer bodyPartSprite in bodyPartsSprites)
            {
                bodyPartSprite.color = new Color(1.0f - (float)(health - phaseHealthLimit[0]) / (maxHealth - phaseHealthLimit[0]),
                    bodyPartSprite.color.g, 0);
            }
        }
        else if (health > phaseHealthLimit[1])
        {
            foreach (SpriteRenderer bodyPartSprite in bodyPartsSprites)
            {
                bodyPartSprite.color = new Color(bodyPartSprite.color.r,
                    (float)(health - phaseHealthLimit[1]) / (phaseHealthLimit[0] - phaseHealthLimit[1]), 0);
            }
        }

        if (phaseThreePartActivated[0] && !phaseTransition)
        {
            if (warningAlpha == 1)
            {
                warningSpeed = -warningSpeed;
            }
            else if (warningAlpha <= 0.3)
            {
                warningSpeed = -warningSpeed;
            }

            warningAlpha += warningSpeed * Time.deltaTime;
            warningAlpha = Mathf.Clamp01(warningAlpha);

            warningSprite.color = new Color(warningSprite.color.r, warningSprite.color.g, warningSprite.color.b, warningAlpha);
        } else if (phaseThreePartActivated[0] && phaseTransition)
        {
            if (warningAlpha == 1)
            {
                warningSpeed = -warningSpeed;
            }

            warningAlpha += warningSpeed * Time.deltaTime;
            warningAlpha = Mathf.Clamp01(warningAlpha);

            warningSprite.color = new Color(warningSprite.color.r, warningSprite.color.g, warningSprite.color.b, warningAlpha);
        } else if (!phaseThreePartActivated[0] && warningAlpha != 0)
        {
            if (warningAlpha == 1)
            {
                warningSpeed = -warningSpeed;
            }

            warningAlpha += warningSpeed * Time.deltaTime;
            warningAlpha = Mathf.Clamp01(warningAlpha);

            warningSprite.color = new Color(warningSprite.color.r, warningSprite.color.g, warningSprite.color.b, warningAlpha);
        }

        if (playerControl.hp <= 0)
        {
            StopCoroutine(routine);
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Vector2 move = Vector3.up * movementSpeed * direction * Time.deltaTime;

            if (direction == 1 && rb2d.position.y + move.y > max.y - boundaryOffset)
            {
                direction = -1;
            }

            if (direction == -1 && rb2d.position.y + move.y < min.y + boundaryOffset)
            {
                direction = 1;
            }

            rb2d.MovePosition(rb2d.position + move);
        }

        if (initialMovement)
        {
            timePassed += Time.deltaTime;
            rb2d.position = Vector2.Lerp(originalPos, new Vector2(6f, 0f), timePassed / initialMovementDuration);
        }

        if (phaseThreePartActivated[0] && !phaseTransition)
        {
            timePassed += Time.deltaTime;
            rb2d.position = Vector2.Lerp(originalPos, new Vector2(9f, originalPos.y), timePassed / phaseThreeMovementDuration[0]);

            if (timePassed > phaseThreeMovementDuration[0])
            {
                warningSprite.color = new Color(warningSprite.color.r, warningSprite.color.g, warningSprite.color.b, 0);
                originalPos = rb2d.position;
                timePassed = 0f;
                phaseThreePartActivated[1] = true;
                phaseThreePartActivated[0] = false;
            }
        } else if (phaseThreePartActivated[1] && !phaseTransition)
        {
            timePassed += Time.deltaTime;
            rb2d.position = Vector2.Lerp(originalPos, new Vector2(-11.5f, originalPos.y), timePassed / phaseThreeMovementDuration[1]);

            if (timePassed > phaseThreeMovementDuration[1])
            {
                rb2d.position = new Vector2(15f, rb2d.position.y);
                originalPos = rb2d.position;
                timePassed = 0f;
                phaseThreePartActivated[2] = true;
                phaseThreePartActivated[1] = false;
            }
        } else if (phaseThreePartActivated[2] && !phaseTransition)
        {
            timePassed += Time.deltaTime;
            rb2d.position = Vector3.Lerp(originalPos, new Vector3(6f, originalPos.y, 0f), timePassed / phaseThreeMovementDuration[2]);

            if (timePassed > phaseThreeMovementDuration[2])
            {
                phaseThreePartActivated[2] = false;
                moving = true;
            }
        }
    }

    IEnumerator BossRoutine()
    {
        initialAnimation();
        yield return new WaitForSeconds(initialMovementDuration);
        endInitialAnimation();

        while (health > phaseHealthLimit[0])
        {
            phaseOne();
            yield return new WaitForSecondsOrHealth(phaseTimes[0], this, 0);
        }

        phaseOneEndAnimationPartOne();
        yield return new WaitForSeconds(armFadeDuration);
        phaseOneEndAnimationPartTwo();

        while (health > phaseHealthLimit[1])
        {
            phaseTwo();
            yield return new WaitForSecondsOrHealth(phaseTimes[1], this, 1);
        }

        phaseTwoEndAnimationPartOne();
        yield return new WaitForSeconds(armFadeDuration);
        phaseTwoEndAnimationPartTwo();

        while (health > phaseHealthLimit[2])
        {
            phaseThree();
            yield return new WaitForSecondsOrHealth(phaseTimes[2], this, 2);
        }

        phaseThreeEndAnimationPartOne();
        yield return new WaitForSeconds(bodyFadeDuration + 0.4f);
        phaseThreeEndAnimationPartTwo();
    }

    void initialAnimation()
    {
        phaseTransition = true;
        initialMovement = true;
        moving = false;
        timePassed = 0f;
        rb2d.position = new Vector2(15f, 0f);
        originalPos = rb2d.position;
    }

    void endInitialAnimation()
    {
        phaseTransition = false;
        initialMovement = false;
        moving = true;
        timePassed = 0f;
    }

    void phaseOne()
    {
        Instantiate(attacks[0], new Vector3(playerTrans.position.x + phaseOneLanceXOffset, max.y + phaseOneLanceYOffset, 0), Quaternion.identity);
        Instantiate(attacks[0], new Vector3(playerTrans.position.x + phaseOneLanceXOffset, min.y - phaseOneLanceYOffset, 0), Quaternion.identity);
    }

    void phaseOneEndAnimationPartOne()
    {
        moving = false;
        phaseTransition = true;

        topArm.GetComponent<BossOneArmAnimation>().StartDeath(armFadeDuration);

        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy Attacks");

        foreach (GameObject enemyObject in enemyObjects)
        {
            enemyObject.GetComponent<BossOneLance>().StopLance(armFadeDuration);
        }
    }

    void phaseOneEndAnimationPartTwo()
    {
        moving = true;
        phaseTransition = false;

        bodyPartsSprites.Remove(topArm.GetComponent<SpriteRenderer>());
    }

    void phaseTwo()
    {
        phaseTwoSpikeYOffset = Random.Range(0f, phaseTwoSpikeYOffsetRange);
        phaseTwoSpikeYOffset = Random.Range(0f, 1f) > 0.5 ? phaseTwoSpikeYOffset : -phaseTwoSpikeYOffset;

        float possibleSpikeYPos = playerTrans.position.y + phaseTwoSpikeYOffset;
        if (possibleSpikeYPos > max.y)
        {
            phaseTwoSpikeYOffset = -phaseTwoSpikeYOffset;
        } else if (possibleSpikeYPos < min.y)
        {
            phaseTwoSpikeYOffset = -phaseTwoSpikeYOffset;
        }

        Instantiate(attacks[1], new Vector3(rb2d.position.x - phaseTwoSpikeXOffset, playerTrans.position.y + phaseTwoSpikeYOffset, 0), Quaternion.Euler(0f, 0f, 180f));
    }

    void phaseTwoEndAnimationPartOne()
    {
        moving = false;
        phaseTransition = true;

        botArm.GetComponent<BossOneArmAnimation>().StartDeath(armFadeDuration);

        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy Attacks");

        foreach (GameObject enemyObject in enemyObjects)
        {
            enemyObject.GetComponent<BossOneSpike>().StopSpike(armFadeDuration);
        }
    }

    void phaseTwoEndAnimationPartTwo()
    {
        moving = true;
        phaseTransition = false;
        boundaryOffset = 2.3f;

        bodyPartsSprites.Remove(botArm.GetComponent<SpriteRenderer>());
    }

    void phaseThree()
    {
        moving = false;
        warningAlpha = 0.3f;
        originalPos = rb2d.position;
        timePassed = 0f;
        phaseThreePartActivated[0] = true;
    }

    void phaseThreeEndAnimationPartOne()
    {
        moving = false;
        phaseTransition = true;

        body.GetComponent<BossOneBodyAnimation>().StartDeath(bodyFadeDuration);
        body.GetComponent<CircleCollider2D>().enabled = false;
        middle.GetComponent<BossOneBodyAnimation>().StartDeath(bodyFadeDuration);
        middle.GetComponent<PolygonCollider2D>().enabled = false;
        mark.GetComponent<BossOneBodyAnimation>().StartDeath(bodyFadeDuration);

        playerControl.bossBoundary = max.x - playerControl.spriteOffset;
    }

    void phaseThreeEndAnimationPartTwo()
    {
        victory.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void takeDamage(int dmg)
    {
        if (!phaseTransition)
        {
            health -= dmg;
        }
    }
}
