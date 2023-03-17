using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpoint : Hittable
{
    public HealthSystem bossHealthSystem;
    public override void Damage(int amount)
    { 
        bossHealthSystem.Damage(amount);
        onGetDamage.Invoke();
    }

    public override void Damage(int amount, Vector2 direction)
    { 
        Damage(amount);
    }
}
