using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsOrHealth : CustomYieldInstruction
{
    private float numSeconds;
    private float startTime;
    private BossController boss;
    private int phase;

    public override bool keepWaiting
    {
        get
        {
            return Time.time - startTime < numSeconds && boss.health > boss.phaseHealthLimit[phase];
        }
    }

    public WaitForSecondsOrHealth(float numSeconds, BossController boss, int phase)
    {
        startTime = Time.time;
        this.numSeconds = numSeconds;
        this.boss = boss;
        this.phase = phase;
    }
}
