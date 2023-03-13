using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public UnityEvent onGetDamage;
    public UnityEvent onDeath;
    
    public void Damage(int amount)
    {
        health -= amount;
        onGetDamage.Invoke();
        if(health <= 0)
            onDeath.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
