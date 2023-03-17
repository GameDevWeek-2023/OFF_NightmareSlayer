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
        if (newHealthSystem != null && ownHealthSystem != null) newHealthSystem.health = Mathf.RoundToInt(newHealthSystem.startHealth * (1f * ownHealthSystem.health / ownHealthSystem.startHealth));
        
        if(Application.isPlaying) Destroy(gameObject);
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        allEnemies.Remove(this);
    }

    public static void ReplaceAll(bool isNightmare)
    {
        if (allEnemies == null) return;
        ReplaceEnemy[] enemies = allEnemies.ToArray();
        foreach (ReplaceEnemy enemy in enemies)
        {
            enemy.Replace(isNightmare);
        }
    }
    
    
    public static void RemoveAll()
    {
        allEnemies = new List<ReplaceEnemy>();
    }
}
