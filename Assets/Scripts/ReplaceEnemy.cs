using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceEnemy : MonoBehaviour
{
    public GameObject normalVersion;
    public GameObject nightmareVersion;
    
    private static List<ReplaceEnemy> allEnemies;

    private Rigidbody2D ownRigidbody;
    private HealthSystem ownHealthSystem;
    
    private void Awake()
    {
        if (allEnemies == null) allEnemies = new List<ReplaceEnemy>();
        if (!allEnemies.Contains(this)) allEnemies.Add(this);

        ownRigidbody = GetComponent<Rigidbody2D>();
        ownHealthSystem = GetComponent<HealthSystem>();
    }
    
    public void Replace(bool isNightmare)
    {
        GameObject newEnemy = Instantiate(isNightmare ? nightmareVersion : normalVersion, transform.position,
            transform.rotation);
        
        Rigidbody2D newRigidbody = newEnemy.GetComponent<Rigidbody2D>();
        if (newRigidbody != null && ownRigidbody != null) newRigidbody.velocity = ownRigidbody.velocity;

        HealthSystem newHealthSystem = newEnemy.GetComponent<HealthSystem>();
        if (newHealthSystem != null && ownHealthSystem != null) newHealthSystem.health = ownHealthSystem.health;
        
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        allEnemies.Remove(this);
    }

    public static void ReplaceAll(bool isNightmare)
    {
        if (allEnemies == null) return;
        for (int i = allEnemies.Count-1; i >= 0; i--)
        {
            ReplaceEnemy enemy = allEnemies[i];
            enemy.Replace(isNightmare);
        }
    }
}
