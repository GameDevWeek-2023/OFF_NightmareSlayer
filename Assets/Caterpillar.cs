using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caterpillar : MonoBehaviour
{
    public SpriteRenderer _spriteRenderer;
    public Sprite Idle;
    public Sprite Compressed;
    private bool idle = true;
    private void Awake()
    {
        StartCoroutine(movement());
    }

    IEnumerator movement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            idle = !idle;
            if (idle) _spriteRenderer.sprite = Idle;
            else _spriteRenderer.sprite = Compressed;
        }
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
