using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleAttack : BossAttack
{
    public RepeatingSpawner[] spawners;
    public override void Attack()
    {
        boss.animator.Play("Sway");
        foreach(RepeatingSpawner spawner in spawners)
        {
            spawner.StartSpawning();
        }
        Invoke("End", 5);
    }

    public void End()
    {
        Finished();

        foreach (RepeatingSpawner spawner in spawners)
        {
            spawner.StopSpawning();
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
