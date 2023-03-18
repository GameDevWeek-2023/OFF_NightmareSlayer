using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private void Start()
    {
        DisableChunk();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        EnableChunk();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        DisableChunk();
    }

    private void EnableChunk()
    {
        foreach (var child in GetAllChildren())
        {
            child.SetActive(true);
        }
        EnemySpawner.RespawnChunk(gameObject);
    }

    private void DisableChunk()
    {
        foreach (var child in GetAllChildren())
        {
            child.SetActive(false);
        }
        EnemySpawner.DespawnChunk(gameObject);
    }

    private List<GameObject> GetAllChildren()
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }

        return children;
    }
}
