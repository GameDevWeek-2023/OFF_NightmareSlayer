using System;
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
        
    }

    private void DisableChunk()
    {
        
    }
}
