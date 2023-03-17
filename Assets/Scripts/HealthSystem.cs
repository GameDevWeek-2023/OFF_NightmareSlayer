using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : Hittable
{
    public int health;
    [HideInInspector]
    public int startHealth;
    private Rigidbody2D rb;
    public UnityEvent onDeath;
    private void Awake()
    {
        startHealth=health;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Damage(int amount)
    {
        health -= amount;
        onGetDamage.Invoke();
        if(health <= 0)
            onDeath.Invoke();
    }

    public override void Damage(int amount, Vector2 direction)
    {
        health -= amount;
        onGetDamage.Invoke();
        if(rb!=null)
            rb.velocity = direction;
        if (health <= 0)
            onDeath.Invoke();
    }
}
