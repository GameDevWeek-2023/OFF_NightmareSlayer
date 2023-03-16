using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public UnityEvent onGetDamage;
    public UnityEvent onDeath;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Damage(int amount)
    {
        health -= amount;
        onGetDamage.Invoke();
        if(health <= 0)
            onDeath.Invoke();
    }

    public void Damage(int amount, Vector2 direction)
    {
        health -= amount;
        onGetDamage.Invoke();
        if(rb!=null)
            rb.velocity = direction;
        if (health <= 0)
            onDeath.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
