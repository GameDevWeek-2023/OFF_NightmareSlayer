using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static List<EnemySpawner> allSpawner;
    public GameObject enemyContaining;

    private GameObject chunk;
    
    private void Awake()
    {
        if (allSpawner == null) allSpawner = new List<EnemySpawner>();
        if (!allSpawner.Contains(this)) allSpawner.Add(this);

        chunk = transform.parent.gameObject;
    }

    private void Despawn()
    {
        if (transform.childCount == 0) return;
        Transform child = transform.GetChild(0);
        Destroy(child.gameObject);
    }

    private void Respawn()
    {
        Despawn();
        GameObject newChild = Instantiate(enemyContaining, transform);

        ReplaceEnemy replaceEnemy = newChild.GetComponent<ReplaceEnemy>();
        if (replaceEnemy != null)
        {
            replaceEnemy.Replace(GameManager.instance.nightmareMode);
        }
    }

    private void OnDestroy()
    {
        allSpawner.Remove(this);
    }

    public static void DespawnAll()
    {
        if (allSpawner == null) return;
        foreach (var spawner in allSpawner)
        {
            spawner.Despawn();
        }
    }

    public static void RespawnAll()
    {
        if (allSpawner == null) return;
        foreach (var spawner in allSpawner)
        {
            spawner.Respawn();
        }
    }

    public static void DespawnChunk(GameObject selectedChunk)
    {
        if (allSpawner == null) return;
        foreach (var spawner in allSpawner)
        {
            if(spawner.chunk == selectedChunk) spawner.Despawn();
        }
    }

    public static void RespawnChunk(GameObject selectedChunk)
    {
        if (allSpawner == null) return;
        foreach (var spawner in allSpawner)
        {
            if(spawner.chunk == selectedChunk) spawner.Respawn();
        }
    }
}
