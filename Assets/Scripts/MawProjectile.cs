using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MawProjectile : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(transform.parent.localScale.x,1,1);
        rigidbody.velocity = transform.right * transform.parent.localScale.x * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerScript.instance.GetDamage();
        }
        Destroy(gameObject);
    }
}
